using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameTile : Tile {
    public enum TileType { ground, water, forest, mountain, border, selected };

    public int x { get; private set; }
    public int y { get; private set; }
    public TileType type { get; private set; }
    public GameObject inGameObject { get; private set; } // gameobject with script InGameObject

    // this method is 'constructor' - used in ScriptableObject.CreateInstance
    public GameTile init(int x, int y, TileType tp, ref Sprite s) {
        this.x = x;
        this.y = y;
        this.type = tp;
        this.sprite = s;

        return this;
    }

    bool isOccupied() {
        return inGameObject != null;
    }

    public void setInGameObject(GameObject _inGameObject) {
        inGameObject = _inGameObject;
      //  Debug.Log(inGameObject.ToString());
    }

    public TileType getType() {
        return type;
    }

    public void clickEvent() {
        if (inGameObject != null) {
          //  inGameObject.Click();
            Debug.Log("clicked");
        }
    }
}
 