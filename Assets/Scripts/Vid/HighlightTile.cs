using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HighlightTile : Tile {
    public enum TileColor { red, green, border };

    public int x { get; private set; }
    public int y { get; private set; }
    public int selectedUnitDistance;
    public int selectedUnitPreviousPath;
    public TileColor tileColor { get; private set; }
    private MapController mapController;

    // this method is 'constructor' - used in ScriptableObject.CreateInstance
    public HighlightTile init(int x, int y, ref MapController mapC) {
        this.x = x;
        this.y = y;
        this.tileColor = TileColor.border;
        this.mapController = mapC;
        this.sprite = mapController.highlightTextures[0];

        return this;
    }

    public void changeColor(TileColor _color)
    {
        switch (_color) {
            case TileColor.red:
                tileColor = _color;
                sprite = mapController.highlightTextures[1];
                break;
            case TileColor.green:
                tileColor = _color;
                sprite = mapController.highlightTextures[2];
                break;
            default:
                tileColor = TileColor.border;
                sprite = mapController.highlightTextures[0];
                break;
        }
    }
}
