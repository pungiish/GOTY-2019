using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class GameState
{
    public static PlayerController selectedPlayer = null;
    private static Unit selectedUnit = null;
    public static Tilemap map = null;
    public static Tilemap highlight = null;

    private static GameTile selectedTile = null;

    public static void Init(Tilemap _map, Tilemap _highlight)
    {
        map = _map;
        highlight = _highlight;
    }
    
    public static Unit SelectedUnit { get { return selectedUnit; }
        set {
            if (value != null)
                GameData.SelectedUnitPanel.transform.position = value.transform.position;
            else if(selectedUnit != null)//value = null, selected Unit != null -> skrijemo Panel na initial position (za tilemap)
                GameData.SelectedUnitPanel.transform.position = GameData.PanelInitial;
            
            selectedUnit = value;
        }
    }

    public static void TileEntered(Vector3Int pos)
    {
        if (selectedUnit != null)
            selectedUnit.DrawMoveLineToDest(pos);
    }

    public static void MouseClicked(Vector3Int pos)
    {
        GameTile posTile = map.GetTile<GameTile>(pos);
        if (selectedUnit == null && posTile.inGameObject == null) //no game action needed
            return;

        if(posTile.inGameObject == null && selectedUnit != null) // premik
        {
            GameTile unitLoc = selectedUnit.TilePos;
            //sync lock?
            if (!SelectedUnit.Move(pos)) //Enota ne more izvesti premika
                return;

            //rezerviramo koncni tile
            posTile.inGameObject = GameObject.Instantiate(GameData.ReservedSpacePanel);
            posTile.inGameObject.transform.position = map.GetCellCenterWorld(pos);
            
            unitLoc.inGameObject = null; //sprostimo prejsnjo lokacijo
            SelectedUnit = null;
        }
        if (posTile.inGameObject != null)
        {
            Unit u = posTile.inGameObject.GetComponent<Unit>();
            if (u == null) //na polju je zgradba ali rezerviran panel
                return;
            else if (u == selectedUnit) //ce kliknemo na izbrano enoto, ne bo izbrana nobena enota vec
            {
                selectedUnit.DrawNoMoveLine();
                SelectedUnit = null;
                UnitHelpFunctions.PathFinding.Clear(highlight, HighlightTile.TileColor.border, true);
            }
            else if (u.player == selectedPlayer)
            {//izbrana enota se menja
                SelectedUnit = u;
                selectedTile = posTile;
                u.ShowPossibleMoves();
            }
            else if(selectedUnit != null) //izbrana enota je od drugega igralca, poskusimo napasti
            {
                selectedUnit.Fight(u);
            }

        }
    }
    

    public static void MovementStart()
    {
        Interlocked.Increment(ref numOfUnitsMoving);
    }
    public static void MovementEnd(Unit u, GameTile endTile)
    {
        if(endTile.inGameObject != null)
            GameObject.Destroy(endTile.inGameObject);

        endTile.inGameObject = u.gameObject;
        
        Interlocked.Decrement(ref numOfUnitsMoving);
    }
    public static bool UnitsMoving()
    {
        int val = numOfUnitsMoving; //load je atomarna operacija
        return val != 0;
    }
    private static int numOfUnitsMoving = 0;
}

