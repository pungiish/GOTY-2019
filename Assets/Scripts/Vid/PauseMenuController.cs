using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour {
    public GameController activeGame;
    public Button resumeButton;
    public Button endGameButton;
    public Button soundButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void changeSound() {
        // turn on/off sound for the whole game (var playSound in GameController)
    }
}
