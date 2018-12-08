using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseController : MonoBehaviour {
    private Vector3Int position;
    public MapController mapController { private get; set; }
    public Tilemap highlightMap { private get; set; }
    public Tilemap map { private get; set; }

    private void OnMouseEnter() {
        position = Vector3Int.zero;
        position.x = (gameObject.transform.position.x < 0) ? (int)gameObject.transform.position.x - 1 : (int)gameObject.transform.position.x;
        position.y = (gameObject.transform.position.y < 0) ? (int)gameObject.transform.position.y - 1 : (int)gameObject.transform.position.y;

        // tukaj se potem glede na vrednost, ce lahko pride ali ne, spremeni tile
        highlightMap.SetTile(position, ScriptableObject.CreateInstance<HighlightTile>().init(position.x, position.y, HighlightTile.TileColor.green, ref mapController.selectedTile));
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnMouseExit() {
        // tukaj se spremeni nazaj na border, ker igralec z misko zapusti tile
        highlightMap.SetTile(position, ScriptableObject.CreateInstance<HighlightTile>().init(position.x, position.y, HighlightTile.TileColor.border, ref mapController.highlightTexture));
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnMouseDown() {
        //Debug.Log(position.ToString());
        (map.GetTile(position) as GameTile).clickEvent();
    }
}
