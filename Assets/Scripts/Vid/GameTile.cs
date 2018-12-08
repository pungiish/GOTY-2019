using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTile : Tile {
    public enum TileColor { red, green };
    public enum TileType { ground, water, forest, mountain, border };

    private int x;
    private int y;
    private TileType type;
    private TileColor colorTag;
    private GameObject warrior; // warrior on this tile (GameObject will be changed to Warrior)
    private GameObject building; // building on this tile

    // this method is 'constructor' - used in ScriptableObject.CreateInstance
    public GameTile init(int x, int y, TileType tp, ref Sprite s) {
        this.x = x;
        this.y = y;
        this.type = tp;
        this.sprite = s;
        this.colorTag = TileColor.green;

        return this;
    }

    bool isOccupiedByWarrior() {
        return warrior != null;
    }

    bool isOccupiedByBuilding() {
        return building != null;
    }

    void changeColor(TileColor c) {
        this.colorTag = c;
    }

    public void setBuilding(GameObject _building) {
        building = _building;
    }

    public void setWarrior(GameObject _warrior) {
        warrior = _warrior;
    }

    public TileType getType() {
        return type;
    }
}
