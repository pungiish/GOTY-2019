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
        Debug.Log("Position: " + position.x + ',' + position.y);

        //risanje crte premika
        GameState.TileEntered(position);
    }

    private void OnMouseExit() {
        mapController.selectMap.SetTile(position, null);
    }

    private void OnMouseDown()
    {
        
        position = Vector3Int.zero;
        position.x = (gameObject.transform.position.x < 0) ? (int)gameObject.transform.position.x - 1 : (int)gameObject.transform.position.x;
        position.y = (gameObject.transform.position.y < 0) ? (int)gameObject.transform.position.y - 1 : (int)gameObject.transform.position.y;
        GameState.MouseClicked(position);
        /*
        GameTile selectedTile = mapController.map.GetTile<GameTile>(position);

        if (selectedTile.inGameObject != null)
        {
            Unit u = selectedTile.inGameObject.GetComponent<Unit>();
            if (u != null && u.player == GameState.selectedPlayer)
            {
                GameState.SelectedUnit = u;
                GameState.SelectedUnit.ShowPossibleMoves();
            }
        }
        else
        {
            if (GameState.SelectedUnit != null && GameState.SelectedUnit.Move(position)) //Move vrne true, ce se premik zacne izvajati
            {
                GameState.SelectedUnit.TilePos.setInGameObject(null);
                selectedTile.setInGameObject(GameState.SelectedUnit.gameObject);
            }
        }
        */

        //selectedTile.clickEvent();
    }
}
