using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnCountdownController : MonoBehaviour {
    private const int maxTime = 15;
    private int fullTime = maxTime;
    private const string countdownText = "Time Left\n";
    private const string countdownOver = "Time's over.";
    public Text countdownTextUI;
    public GameController gameController;

    public void resetAndStart() {
        fullTime = maxTime;
        StartCoroutine(StartCountdown());
    }

    IEnumerator StartCountdown() {
        while (fullTime > 0) {
            countdownTextUI.text = countdownText + fullTime;
            fullTime--;

            yield return new WaitForSeconds(1);
        }

        if (GameState.SelectedUnit != null)
        {
            GameState.SelectedUnit.DrawNoMoveLine();
            GameState.ClearPossibleMoves();
        }
        GameState.selectedPlayer = null;
        GameState.SelectedUnit = null;

        countdownTextUI.text = countdownOver;

        while(GameState.NumOfUnitsInAction() > 0)
            yield return new WaitForSeconds(0.5f);
        gameController.nextTurn();
    }
}
