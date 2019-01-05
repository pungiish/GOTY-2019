using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour {
    public static bool GamePaused = false;
    public GameObject pauseMenuUI;

    public Text soundText;
    private const string soundOff = "SOUND OFF";
    private const string soundOn = "SOUND ON";

    public int mainMenuScene;

    private void changeSoundText() {
        if (GameController.playSound) {
            soundText.text = soundOff;
        }
        else {
            soundText.text = soundOn;
        }
    }

    public void changeSound() {
        GameController.playSound = !GameController.playSound;
        changeSoundText();
    }

    public void resume() {
        pauseMenuUI.SetActive(false);
        GamePaused = false;
        Time.timeScale = 1.0f;
    }

    public void pause() {
        pauseMenuUI.SetActive(true);
        changeSoundText();
        GamePaused = true;
        Time.timeScale = 0.0f;
    }

    public void exitGame() {
        SceneManager.LoadScene(mainMenuScene);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape)) {
            if (GamePaused) {
                resume();
            } else {
                pause();
            }
        }
    }
}
