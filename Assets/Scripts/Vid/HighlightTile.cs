using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightTile : Tile {
    public enum TileColor { red, green, border };

    public int x { get; private set; }
    public int y { get; private set; }
    public int selectedUnitDistance;
    private TileColor tileColor;

    // this method is 'constructor' - used in ScriptableObject.CreateInstance
    public HighlightTile init(int x, int y, TileColor tc, ref Sprite s) {
        this.x = x;
        this.y = y;
        this.tileColor = tc;
        this.sprite = s;

        return this;
    }

    public void changeColor(Color _color) {
        //Debug.Log("ok");
        this.color = _color;
    }
}
