using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseController : MonoBehaviour {
    private Vector3Int position;
    public MapController mapController { private get; set; }
    
    private void OnMouseEnter() {
        if (PauseMenuController.GamePaused) {
            return;
        }

        position = Vector3Int.zero;
        position.x = (gameObject.transform.position.x < 0) ? (int)gameObject.transform.position.x - 1 : (int)gameObject.transform.position.x;
        position.y = (gameObject.transform.position.y < 0) ? (int)gameObject.transform.position.y - 1 : (int)gameObject.transform.position.y;

        Tile selected = ScriptableObject.CreateInstance<Tile>();
        selected.sprite = mapController.selectedTile;
        mapController.selectMap.SetTile(position, selected);

        //risanje crte premika
        GameTile selectedTile = mapController.map.GetTile<GameTile>(position);
        if (selectedTile.inGameObject == null && GameState.selectedUnit != null)
        {
            GameState.selectedUnit.DrawMoveLineToDest(position);
        }
    }

    private void OnMouseExit() {
        mapController.selectMap.SetTile(position, null);
    }

    private void OnMouseDown() {
        if (GameState.UnitMoving == true) //ko se enota premika, se ne more klikniti nicesar
            return;

        position = Vector3Int.zero;
        position.x = (gameObject.transform.position.x < 0) ? (int)gameObject.transform.position.x - 1 : (int)gameObject.transform.position.x;
        position.y = (gameObject.transform.position.y < 0) ? (int)gameObject.transform.position.y - 1 : (int)gameObject.transform.position.y;

        GameTile selectedTile = mapController.map.GetTile<GameTile>(position);

        if(selectedTile.inGameObject != null)
        {
            Unit u = selectedTile.inGameObject.GetComponent<Unit>();
            Debug.Log("Selected unit changed");
            if (u != null && u.player == GameState.selectedPlayer)
            {
                GameState.selectedUnit = u;
                GameState.selectedUnit.ShowPossibleMoves();
            }
        }
        else
        {
            if (GameState.selectedUnit != null)
            {
                GameState.selectedUnit.TilePos.setInGameObject(null);
                selectedTile.setInGameObject(GameState.selectedUnit.gameObject);
                GameState.selectedUnit.Move(position);
            }
        }

        //selectedTile.clickEvent();
    }
}
