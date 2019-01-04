using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {
    public GameObject startMenuUI;
    public GameObject selectPlayersUI;
    public GameObject selectWarriorsUI;
    public Text selectWarriorsPlayerText;
    public GameObject selectMapUI;
    public GameObject countdownUI;

    private bool sound = true;
    private int playerTurn = 0;
    private int numberOfPlayers;

    public Text soundText;
    private const string soundOff = "SOUND OFF";
    private const string soundOn = "SOUND ON";
    private const string playerr = "Player ";

    private const string players = "players";
    private const string player = "player";
    private const string map = "map";    

    private void changeSoundText() {
        if (sound) {
            soundText.text = soundOff;
        }
        else {
            soundText.text = soundOn;
        }
    }

    public void changeSound() {
        sound = !sound;
        changeSoundText();
    }

    void Start () {
        PlayerPrefs.DeleteAll();
        changeSoundText();
	}

    public void exitGame() {
        Application.Quit();
    }

    public void playGame() {
        startMenuUI.SetActive(false);
        selectPlayersUI.SetActive(true);
    }

    public void selectPlayers(int number) {
        // save the number of players to the PlayerPrefs so the value in send over to the game scene
        PlayerPrefs.SetInt(players, number);
        numberOfPlayers = number;

        playerTurn = 0;
        selectPlayersUI.SetActive(false);
        selectWarriorsUI.SetActive(true);
        selectWarriorsPlayerText.text = playerr + (playerTurn + 1);
    }

    public void selectPlayersBack() {
        selectPlayersUI.SetActive(false);
        startMenuUI.SetActive(true);
    }

    public void selectWarriors(int number) {
        PlayerPrefs.SetInt(player + playerTurn, number);

        if (++playerTurn < numberOfPlayers) {
            selectWarriorsPlayerText.text = playerr + (playerTurn + 1);
        } else {
            selectWarriorsUI.SetActive(false);
            selectMapUI.SetActive(true);
        }
    }

    public void selectWarriorsBack() {
        selectWarriorsUI.SetActive(false);
        selectPlayersUI.SetActive(true);
    }

    public void selectMap(int number) {
        PlayerPrefs.SetInt(map, number);

        selectMapUI.SetActive(false);
        countdownUI.SetActive(true);
    }

    public void selectMapBack() {
        playerTurn = 0;
        selectWarriorsPlayerText.text = playerr + (playerTurn + 1);
        selectMapUI.SetActive(false);
        selectWarriorsUI.SetActive(true);
    }
}
