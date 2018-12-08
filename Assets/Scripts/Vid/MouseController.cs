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

        //this.gameObject.GetComponent<MeshRenderer>().enabled = true;

        Tile selected = ScriptableObject.CreateInstance<HighlightTile>();
        selected.sprite = mapController.selectedTile;
        mapController.selectMap.SetTile(position, selected);

        //mapController.highlight.SetTileFlags(position, TileFlags.None);
        //mapController.highlight.SetColor(position, Color.green);

        //this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnMouseExit() {
        //highlightMap.SetTile(position, ScriptableObject.CreateInstance<HighlightTile>().init(position.x, position.y, HighlightTile.TileColor.border, ref mapController.highlightTexture));
        //this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnMouseDown() {
        //Debug.Log(position.ToString());
        (mapController.map.GetTile(position) as GameTile).clickEvent();
    }
}
