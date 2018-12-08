using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MouseController : MonoBehaviour {
    private Vector3Int position;
    public MapController mapController { private get; set; }
    public Tilemap highlightMap { private get; set; }

    private void OnMouseEnter() {
        position = Vector3Int.zero;
        position.x = (gameObject.transform.position.x < 0) ? (int)gameObject.transform.position.x - 1 : (int)gameObject.transform.position.x;
        position.y = (gameObject.transform.position.y < 0) ? (int)gameObject.transform.position.y - 1 : (int)gameObject.transform.position.y;
        highlightMap.SetTile(position, ScriptableObject.CreateInstance<GameTile>().init(position.x, position.y, GameTile.TileType.selected, ref mapController.selectedTile));
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnMouseExit() {
        highlightMap.SetTile(position, ScriptableObject.CreateInstance<GameTile>().init(position.x, position.y, GameTile.TileType.border, ref mapController.highlightTexture));
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
