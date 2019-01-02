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

        countdownTextUI.text = countdownOver;
        gameController.nextTurn();
    }
}
