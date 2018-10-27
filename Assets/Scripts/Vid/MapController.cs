using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour {
    public int width;
    public int height;
    public Tilemap map;
    public Tile ground;

    /*MapController(int w, int h) {
        width = w;
        height = h;

        map = new Tilemap();
        map.size = new Vector3Int(width, height, 0);

        startMapGenerator();
    }*/

    private void Start() {
        map.size = new Vector3Int(width, height, 0);

        startMapGenerator();
    }

    private void startMapGenerator() {
        map.SetTile(new Vector3Int(0, 0, 0), new GameTile(0, 0, GameTile.TileType.ground, ground.sprite));
        // map generation here
    }

    public GameTile getTileAt(int x, int y) {
        return map.GetTile(new Vector3Int(x, y, 0)) as GameTile;
    }

    public void showTileBorders() {

    }

    public void hideTileBorders() {

    }
}
