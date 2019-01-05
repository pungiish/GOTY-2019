using System.Collections;
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
    }

    public void ShowPossibleMoves()
    {
        UnitHelpFunctions.PathFinding.FindPossibleMoves(TilePos.x, TilePos.y, MovePoints, 0, player.Map, player.HighlightMap);
    }
    
    LinkedList<Vector3Int> movePath = null;
    bool moving = false;

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
            int dbg = MovePoints;
            MovePoints -= player.HighlightMap.GetTile<HighlightTile>(pos).selectedUnitDistance;
            Debug.Log("MovePoints: " + dbg + "   " + MovePoints);
            UnitHelpFunctions.PathFinding.Clear(player.HighlightMap, HighlightTile.TileColor.border, true);
            GameState.MovementStart();
            TilePos = Map.GetTile<GameTile>(pos); //premaknemo pozicijo se preden pridemo na koncno pozicijo
            this.DrawNoMoveLine();
            moving = true;
            return true;
        }
    }

    // Update is called once per frame
    void Update () {
        if (moving == true && movePath != null)
        {
            float speed = 3.0f;
            float step = speed * Time.deltaTime;
            Vector3 currPos = this.transform.position;
            Vector3 targetPos = Map.GetCellCenterWorld(movePath.First.Value);
            
            if (currPos.Equals(targetPos))
            {
                movePath.RemoveFirst();
                if(movePath.Count == 0) //premik koncan
                {
                    movePath = null;
                    moving = false;
                    GameState.MovementEnd(this, TilePos);
                    return;
                }
                else
                {
                    targetPos = Map.GetCellCenterWorld(movePath.First.Value);
                }
            }
            this.transform.position = Vector3.MoveTowards(currPos, targetPos, step);
        }
	}
    
    public void DrawMoveLineToDest(Vector3Int dest)
    {
        //ce se enota premika, se linija ne kaze in ne spreminja
        if (moving == true)
            return;

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
    
    public void Fight(Unit opponent)
    {
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
                return;
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

        //napadalec ima prednost v prvem krogu napada

        for (int i = 0; i < 2; ++i)
        {
            //udari prva enota - pri prvem udarcu ima "bonus presenecenja, dodatnih 50% skode
            if (i == 0)
                opponent.HealthPoints -= (float)((thisDamage * 1.5) / (1 + 0.1 * opponent.Defence));
            else
                opponent.HealthPoints -= (float)(thisDamage / (1 + 0.1 * opponent.Defence));

            if (opponent.HealthPoints <= 0.0)   //nasprotnik unicen
            {
                Debug.Log("Rund " + i.ToString() + " " + this.ToString() + " | " + opponent.ToString());
                return;
            }
            opponent.healthBar.setHealth(opponent.HealthPoints);
            
            //druga enota
            if (oppDamage > 0) //ce je napadalec v dosegu orozja
                this.HealthPoints -= (float)(oppDamage / (1 + 0.1 * this.Defence));
            
            if (HealthPoints <= 0.0) //napadalec unicen
                return;

            this.healthBar.setHealth(this.HealthPoints);
        }
        
    }
    
}
