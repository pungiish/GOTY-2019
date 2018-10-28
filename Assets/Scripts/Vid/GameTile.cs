﻿using System.Collections;
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
    // private Warrior warrior; is warrior on this tile

    public GameTile(int x, int y, TileType tp, Sprite s) {
        this.x = x;
        this.y = y;
        this.type = tp;
        this.sprite = s;
        this.colorTag = TileColor.green;
    }

    bool isOccupied() {
        // return warrior != null;
        return false;
    }

    void changeColor(TileColor c) {
        this.colorTag = c;
    }
}
