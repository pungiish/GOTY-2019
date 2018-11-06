using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    //private List<PlayerController> players;
    private bool playSound;
    public MapController gameMap;
    public int width;
    public int height;
    
	void Start () {
        generateMap();
	}

    private void generateMap() {
        gameMap.startMapGenerator(width, height);
    }
}
