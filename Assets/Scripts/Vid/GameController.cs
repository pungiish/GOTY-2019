﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    //private List<PlayerController> players;
    private bool playSound;
    public MapController gameMap;
    public int width;
    public int height;
    public static int numberOfPlayers = 2; // ti dve spremenljivki se inicializirata po game menu
    public static int[] selectedWarriorAndBuilding = { 0, 1 };
    
	void Start () {
        generateMap();
	}

    private void generateMap() {
        gameMap.startMapGenerator(width, height);
    }
}
