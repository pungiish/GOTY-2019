using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseController : MonoBehaviour {
    private Vector3Int position;
    public MapController mapController { private get; set; }

    private void OnMouseEnter() {
        position = Vector3Int.zero;
        position.x = (gameObject.transform.position.x < 0) ? (int)gameObject.transform.position.x - 1 : (int)gameObject.transform.position.x;
        position.y = (gameObject.transform.position.y < 0) ? (int)gameObject.transform.position.y - 1 : (int)gameObject.transform.position.y;

        Tile selected = ScriptableObject.CreateInstance<Tile>();
        selected.sprite = mapController.selectedTile;
        mapController.selectMap.SetTile(position, selected);

        //Debug.Log("here");
        //mapController.highlight.SetTileFlags(position, TileFlags.None);
        //mapController.highlight.SetColor(position, Color.green);
    }

    private void OnMouseExit() {
        mapController.selectMap.SetTile(position, null);
    }

    private void OnMouseDown() {
        //Debug.Log(position.ToString());
        (mapController.map.GetTile(position) as GameTile).clickEvent();
    }
}
