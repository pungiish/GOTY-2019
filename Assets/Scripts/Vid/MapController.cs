using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour {
    public Sprite[] groundTextures;
    public Sprite[] waterTextures;
    public Sprite[] forestTextures;
    public Sprite[] mountainTextures;
    public Sprite highlightTexture;
    public GameObject[] warriors;
    public GameObject[] buildings;
    public GameObject selected;
    public Sprite selectedTile;

    private int width;
    private int height;
    public Tilemap map;
    public Tilemap highlight;
    public Tilemap selectMap;

    public void startMapGenerator(int w, int h, List<PlayerController> playerList) {
        width = w;
        height = h;

        map.size = new Vector3Int(width, height, 0);
        highlight.size = new Vector3Int(width, height, 0);

        MapGenerator mapGenerator = new MapGenerator(width, height, 0, ref map, ref highlight, ref selectMap, this, playerList);
        mapGenerator.generateMap();

        //hideTileBorders();
    }

    public GameTile getTileAt(Vector3Int position) {
        return map.GetTile(position) as GameTile;
    }

    public void showTileBorders() {
        highlight.gameObject.SetActive(true);
    }

    public void hideTileBorders() {
        highlight.gameObject.SetActive(false);
    }
}