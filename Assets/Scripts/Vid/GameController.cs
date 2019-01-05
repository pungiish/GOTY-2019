using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    //private List<PlayerController> players;
    public static bool playSound;
    public MapController gameMap;
    public List<PlayerController> Players { get; private set; } 
    public int width;
    public int height;
    public static int numberOfPlayers = 2; // ta spremenljivka se inicializira po game menu
    public static List<int> selectedWarriorAndBuilding = new List<int>();
    public static List<bool> alivePlayers = new List<bool>();
    public static List<GameObject> healthBars = new List<GameObject>();
    public GameObject healthBar;

    private int currentTurn = -1;
    public Text turnText;
    private const string onTurn = "On turn\n";
    public TurnCountdownController turnCountdownController;
    public Text turnChangeText;

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

    public GameObject PrefabsContainer;
    public GameObject lineRendObject;

    private LineRendererHandler lineHandler;

    // v playerprefs so shranjeni podatki:
    // stevilo igralcev v "players"
    // izbrana rasa v "playerX" (X je index igralca)
    // izbrana mapa v "map"
    // ko bo grafika integrirana v igro, se morajo samo te vrednosti prebrati in uporabati

    void Start ()
    {
        Players = new List<PlayerController>();
        Players.Capacity = numberOfPlayers;
        
        lineHandler = gameObject.AddComponent<LineRendererHandler>().Init(0.2f, Color.blue, lineRendObject);
        {
            UnitPrefabs prefabs = PrefabsContainer.GetComponent<UnitPrefabs>();
            GameData.Init(numberOfPlayers, prefabs, healthBar);
        }

        for (int i = 0; i < numberOfPlayers; i++)
        {
            Players.Add(gameObject.AddComponent<PlayerController>().Init("Player1", gameMap, PlayerController.Tribe.A, lineHandler));
            initPlayer(i, i % 2);
        }
        generateMap();

        GameState.Init(gameMap.map, gameMap.highlight);

        nextTurn();
    }

    private IEnumerator ChangeTurnTextAppear() {
        turnChangeText.text = "Player " + (currentTurn + 1) + " turn!";
        turnChangeText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);

        turnChangeText.gameObject.SetActive(false);

        yield return null;
    }

    // call this to change to next turn
    public void nextTurn() {
        do
        {
            currentTurn = (currentTurn + 1) % numberOfPlayers;
        } while (!alivePlayers[currentTurn]);

        if (GameState.SelectedUnit != null)
            GameState.SelectedUnit.DrawNoMoveLine();

        GameState.selectedPlayer = Players[currentTurn];
        GameState.SelectedUnit = null;

        Players[currentTurn].StartTurn();

        turnText.text = onTurn + (currentTurn + 1);
        turnCountdownController.resetAndStart();

        StartCoroutine(ChangeTurnTextAppear());
    }

    // call this to get player on the turn
    public int playerOnTurn() {
        return currentTurn;
    }

    private void generateMap() {
        gameMap.startMapGenerator(width, height, Players);
    }

    public void openPauseMenu() {

    }
}
