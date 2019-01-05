using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour {
    public int menuScene;
    public Text resut;

    private void Start() {
        // prebere se result iz playerprefs, shrani se na koncu igre
    }

    public void goToMainMenu() {
        SceneManager.LoadScene(menuScene);
    }
}
