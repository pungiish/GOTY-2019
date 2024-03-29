﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Threading;

public class Unit : MonoBehaviour
{
   // public GameObject GameObj = null;
   // private static Int32[,] distanceMap;
    
    public int FightRange { get; private set;}
    public int BaseRanged { get; private set; }
    public int BaseMelee { get; private set; }
    public float HealthPoints { get; private set; }
    public int Defence { get; private set; }
    public int MovePoints { get; private set; }

    private int UnitDataIndex;

    public GameTile TilePos { get; private set; }
    public PlayerController player { get; private set; }
    private Tilemap Map;
    private HealthBarController healthBar;

    bool fightThisRound = false;
    int action = 0; //when unit is in action, it cannot go into another action
    
    // Use this for initialization
   void Start () {}

    public Unit Init(PlayerController player, Vector3 pos, GameTile tile, GameData.UnitType type)
    {
        pos.z -= 2.0f;
        this.transform.position = pos;

        this.player = player;
        this.TilePos = tile;
        Map = player.Map;

        UnitDataIndex = GameData.getIndex((int)player.PlayerTribe, type);

        FightRange = GameData.FightRange[UnitDataIndex];
        BaseRanged = GameData.BaseRanged[UnitDataIndex];
        BaseMelee = GameData.BaseMelee[UnitDataIndex];
        HealthPoints = GameData.Health[UnitDataIndex];
        Defence = GameData.Defence[UnitDataIndex];

        GameObject hBar = Instantiate(GameData.HealthBar, this.transform);
        hBar.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        hBar.transform.localPosition = new Vector3(0.0f, -0.55f, -1.0f);
        healthBar = hBar.GetComponent<HealthBarController>();
        healthBar.setHealthBarColors(Color.yellow, Color.magenta);
        healthBar.SetMaxHealth(HealthPoints);
        healthBar.setHealth(HealthPoints);
       
        return this;
    }

    public void StartTurn()
    {
        this.MovePoints = GameData.BaseMoveRanges[UnitDataIndex];
        this.fightThisRound = false;
    }

    public bool ShowPossibleMoves()
    {
        var a = action; //Load is atomic for 32 bit
        if (a == 0)
        { //if no action with the unit, possible moves can be shown
            UnitHelpFunctions.PathFinding.FindPossibleMoves(TilePos.x, TilePos.y, MovePoints, 0, player.Map, player.HighlightMap);
            return true;
        }
        else
        {
            return false;
        }
    }
    
    LinkedList<Vector3Int> movePath = null;
    //bool moving = false;

    //function changes units position immediately (even if it needs some time to reach it)
    public bool Move(Vector3Int pos)
    {
        UnitHelpFunctions.PathFinding.FindPossibleMoves(TilePos.x, TilePos.y, MovePoints, 0, player.Map, player.HighlightMap, false);
        movePath = UnitHelpFunctions.PathFinding.GetMovePositions(player.HighlightMap, pos);

        if (movePath == null)
        {
            return false;
        }
        else
        {
            if(Interlocked.CompareExchange(ref action, 0, 1) == 1)
            {
                return false; // enota ze izvaja neko akcijo
            }
            MovePoints -= player.HighlightMap.GetTile<HighlightTile>(pos).selectedUnitDistance;
            UnitHelpFunctions.PathFinding.Clear(player.HighlightMap, HighlightTile.TileColor.border, true);
            GameState.ActionStart();
            TilePos = Map.GetTile<GameTile>(pos); //premaknemo pozicijo se preden pridemo na koncno pozicijo
            this.DrawNoMoveLine();
            StartCoroutine(MoveCoroutine());
            return true;
        }
    }

    private IEnumerator MoveCoroutine()
    {
        if (movePath == null)
            yield break;

        float speed = 3.0f;
        float step = speed * Time.deltaTime;

        Vector3 currPos = this.transform.position;
        Vector3 targetPos = Map.GetCellCenterWorld(movePath.First.Value);

        while (true)
        {
            currPos = this.transform.position;

            if (currPos.Equals(targetPos))
            {
                movePath.RemoveFirst();
                if(movePath.Count == 0) //premik koncan
                    break;

                targetPos = Map.GetCellCenterWorld(movePath.First.Value);
            }
            this.transform.position = Vector3.MoveTowards(currPos, targetPos, step);
            yield return 0;
        }
        this.transform.position = Vector3.MoveTowards(currPos, targetPos, step);
        movePath = null;
        Interlocked.Exchange(ref action, 0);
        GameState.ActionEnd(this, TilePos);
    }

    void Update() { }
    
    public void DrawMoveLineToDest(Vector3Int dest)
    {
        //ce se enota premika, se linija ne kaze in ne spreminja
        {
            var a = action;
            if (a == 1)
                return;
        }

        movePath = UnitHelpFunctions.PathFinding.GetMovePositions(player.HighlightMap, dest);
        if (movePath == null || movePath.Count < 2)
        {
            player.LineHandler.DrawLine(null); //pot ne obstaja
            return;
        }

        Vector3[] pos = new Vector3[movePath.Count];
        LinkedListNode<Vector3Int> node = movePath.First;
        Vector3 height = new Vector3(0.0f, 0.0f, -1.0f);

        for (int i = 0; i < pos.Length; ++i)
        {
            pos[i] = Map.GetCellCenterWorld(node.Value) + height;
            node = node.Next;
        }
        player.LineHandler.DrawLine(pos);

    }
    public void DrawNoMoveLine()
    {
        player.LineHandler.DrawLine(null);
    }
    
    public bool Fight(Unit opponent)
    {
        if (this.fightThisRound) //enota lahko napada samo 1x na rundo
            return false;

        int thisX = TilePos.x;
        int thisY = TilePos.y;
        int oppX = opponent.TilePos.x;
        int oppY = opponent.TilePos.y;

        //izracun razdalje med enotama
        int dist = (thisX - oppX) * (thisX - oppX) + (oppY - oppY) * (thisY - oppY);

        int thisDamage, oppDamage;
        //ranged fight
        if (dist > 1)
        {
            if (dist > this.FightRange) //ne pride do napada
               return false;
            else
                thisDamage = this.BaseRanged;

            if (dist > opponent.FightRange)
                oppDamage = 0; //nasprotnik ne more odgovoriti na napad
            else
                oppDamage = opponent.BaseRanged;
        }
        else //melee fight
        {
            thisDamage = this.BaseMelee;
            oppDamage = opponent.BaseMelee;
        }

        if (Interlocked.CompareExchange(ref action, 1, 0) != 0) //ena akcija ze poteka
            return false;

        this.fightThisRound = true;
        this.MovePoints = 0; //po napadu se enota ne more vec premikati
        GameState.ActionStart();
        UnitHelpFunctions.PathFinding.Clear(player.HighlightMap, HighlightTile.TileColor.border, true);
        StartCoroutine(FightCoroutine(opponent, thisDamage, oppDamage));
        return true;
    }

    private IEnumerator FightCoroutine(Unit opponent, int thisDamage, int oppDamage)
    {
        FightingUIController UIController = new FightingUIController(this, opponent);
        //napadalec ima prednost v prvem krogu napada
        const int NumOfRounds = 2;
        float currHitDamage;
        for (int i = 0; i < NumOfRounds; ++i)
        {
            yield return new WaitForSeconds(0.5f);

            //udari prva enota - pri prvem udarcu ima "bonus presenecenja, dodatnih 50% skode
            if (i == 0)
                currHitDamage = (float)((thisDamage * 1.5) / (1 + 0.1 * opponent.Defence));
            else
                currHitDamage = (float)(thisDamage / (1 + 0.1 * opponent.Defence));

            UIController.PlayerAttack(FightingUIController.Attacker.unit1, currHitDamage);

            opponent.HealthPoints -= currHitDamage;
            if (opponent.HealthPoints <= 0.0)   //nasprotnik unicen
            {
                opponent.healthBar.setHealth(0.0f);
                opponent.player.UnitDestroyed(opponent);
                UIController.FightingEnd(false, true);
                Interlocked.Exchange(ref action, 0);
                GameState.ActionEnd(null, null);
                yield break;
            }
            opponent.healthBar.setHealth(opponent.HealthPoints);
            yield return new WaitForSeconds(0.5f);

            //druga enota
            if (oppDamage > 0) //ce je napadalec v dosegu orozja
                currHitDamage = (float)(oppDamage / (1 + 0.1 * this.Defence));
            else
                currHitDamage = 0.0f; //drugace je 0

            UIController.PlayerAttack(FightingUIController.Attacker.unit2, currHitDamage);
            this.HealthPoints -= currHitDamage;

            if (HealthPoints <= 0.0)
            { //napadalec unicen
                this.healthBar.setHealth(0.0f);
                this.player.UnitDestroyed(this);
                UIController.FightingEnd(true, false);
                GameState.ActionEnd(null, null);
                Interlocked.Exchange(ref action, 0);
                yield break;
            }

            this.healthBar.setHealth(this.HealthPoints);
        }
        UIController.FightingEnd(false, false);
        GameState.ActionEnd(null, null);
        Interlocked.Exchange(ref action, 0);
    }
}
