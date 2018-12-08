using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour {
    public Text countdownText;
    public Text pressToPlayText;
    public int scene;
    private bool loadScene = false;
	
	void Update() {
        if (!loadScene && Input.GetKeyDown(KeyCode.Space)) {
            loadScene = true;
            pressToPlayText.enabled = false;
            StartCoroutine(LoadNewScene());
        }
    }

    IEnumerator LoadNewScene() {
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;

        for (int i = 3; i >= 0; i--) {
            if (i != 0) {
                countdownText.text = i.ToString();
            } else {
                countdownText.text = "Start!";
            }
            yield return new WaitForSeconds(1);
        }

        async.allowSceneActivation = true;

        while (!async.isDone) {
            yield return null;
        }
    }
}
