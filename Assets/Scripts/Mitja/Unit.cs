using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class Unit : MonoBehaviour
{
   // public GameObject GameObj = null;
    private static Int32[,] distanceMap;
    
    public int Attack { get; private set;}
    public int MovePoints { get; private set; }

    protected int MoveRange = -1;

    public GameTile TilePos { get; private set; }
    public PlayerController player { get; private set; }
    private Tilemap Map;
    
    // Use this for initialization
   void Start () {}

    public Unit Init(PlayerController player, Vector3 pos, GameTile tile)
    {
        //this.transform.SetParent(pos.gameObject.transform, true);
        pos.z -= 2.0f;
        this.transform.position = pos;
        
        this.player = player;
        this.TilePos = tile;
        Map = player.Map;
        return this;
    }

    public void ShowPossibleMoves()
    {
        MoveRange = 30;
        UnitHelpFunctions.PathFinding.FindPossibleMoves(TilePos.x, TilePos.y, MoveRange, 0, player.Map, player.HighlightMap);
    }
    
    LinkedList<Vector3Int> movePath = null;
    bool moving = false;

    //function changes units position immediately (even if it needs some time to reach it)
    public bool Move(Vector3Int pos)
    {
        UnitHelpFunctions.PathFinding.FindPossibleMoves(TilePos.x, TilePos.y, MoveRange, 0, player.Map, player.HighlightMap, false);
        movePath = UnitHelpFunctions.PathFinding.GetMovePositions(player.HighlightMap, pos);

        if (movePath == null)
        {
            return false;
        }
        else
        {
            GameState.MovementStart();
            TilePos = Map.GetTile<GameTile>(pos); //premaknemo pozicijo se preden na koncno pozicijo
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
                    ShowPossibleMoves();
                    GameState.MovementEnd();
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

}
