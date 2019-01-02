﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    //private List<PlayerController> players;
    private bool playSound;
    public MapController gameMap;
    public int width;
    public int height;
    public static int numberOfPlayers = 2; // ti dve spremenljivki se inicializirata po game menu
    public static List<int> selectedWarriorAndBuilding = new List<int>();
    public static List<bool> alivePlayers = new List<bool>();
    public static List<GameObject> healthBars = new List<GameObject>();
    public GameObject healthBar;

    private int currentTurn = -1;
    public Text turnText;
    private const string onTurn = "On turn\n";
    public TurnCountdownController turnCountdownController;

    private void initPlayer(int index, int selectedWB) {
        selectedWarriorAndBuilding.Add(selectedWB);
        alivePlayers.Add(true);
        GameObject hb = GameObject.Instantiate(healthBar);
        hb.transform.parent = Camera.main.transform;
        Vector3 newPosition = new Vector3(0.29f, 0.915f, 2.0f);

        switch (index) {
            case 1:
                newPosition.x = 0.7f;
                break;
            case 2:
                newPosition.y = 0.115f;
                break;
            case 3:
                newPosition.x = 0.7f;
                newPosition.y = 0.115f;
                break;
            default:
                break;
        }

        hb.transform.position = Camera.main.ViewportToWorldPoint(newPosition);
        healthBars.Add(hb);
    }
    
	void Start () {
        for (int i = 0; i < numberOfPlayers; i++) {
            initPlayer(i, i % 2);
        }
        generateMap();
        nextTurn();
    }

    // call this to change to next turn
    public void nextTurn() {
        currentTurn = (currentTurn + 1) % numberOfPlayers;
        if (!alivePlayers[currentTurn]) {
            nextTurn();
        }

        turnText.text = onTurn + (currentTurn + 1);
        turnCountdownController.resetAndStart();
    }

    // call this to get player on the turn
    public int playerOnTurn() {
        return currentTurn;
    }

    private void generateMap() {
        gameMap.startMapGenerator(width, height);
    }
}
