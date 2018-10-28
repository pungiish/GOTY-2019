using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour {
    public int width;
    public int height;
    public Tilemap map;
    public Tilemap highlight;
    public Tile ground;
    public Tile border;

    /*MapController(int w, int h) {
     * v konstruktorju se bomo nastavili vsi podatki za MapController in potem se bo klical startMapGenerator
        width = w;
        height = h;

        map = new Tilemap();
        map.size = new Vector3Int(width, height, 0);

        startMapGenerator();
    }*/

    private void Start() { // test purposes
        map.size = new Vector3Int(width, height, 0);
        highlight.size = new Vector3Int(width, height, 0);

        startMapGenerator();
    }

    private void startMapGenerator() {

        map.SetTile(new Vector3Int(0, 0, 0), new GameTile(0, 0, GameTile.TileType.ground, ground.sprite)); // test purposes
        //highlight.SetTile(new Vector3Int(0, 0, 0), new GameTile(0, 0, GameTile.TileType.border, border.sprite)); // test purposes

        // map generation here
        MapGenerator.generateMap(ref map);
        //hideTileBorders();
        //showTileBorders();
    }

    public GameTile getTileAt(int x, int y) {
        return map.GetTile(new Vector3Int(x, y, 0)) as GameTile;
    }

    public void showTileBorders() {
        highlight.gameObject.SetActive(true);
    }

    public void hideTileBorders() {
        highlight.gameObject.SetActive(false);
    }

    private void OnMouseOver() {
        // tile na tej lokaciji bo spremenil barvo glede na to, če je veljavna poteza
    }

    private void OnMouseDown() {
        // tile se bo izbral, preveriti bo potrebno, če je veljavna poteza
    }
}