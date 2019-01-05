using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour {
    public GameController gameController;
    public float speed;
    private int maxX;
    private int maxY;
    private int screenWidth;
    private int screenHeight;

	void Start () {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        maxX = gameController.width / 2 - (int)Camera.main.orthographicSize;
        maxY = gameController.height / 2 - (int)Camera.main.orthographicSize / 2;
    }
	
	void Update () {
        if (PauseMenuController.GamePaused) {
            return;
        }

        Vector3 camPos = transform.position;
        int tempX = (int)((Input.mousePosition.x / Screen.width) * 100);
        int tempY = (int)((Input.mousePosition.y / Screen.height) * 100);

        if (tempX > 90 && camPos.x < maxX) { // right
            camPos.x += speed * Time.deltaTime;
        }
        if (tempX < 10 && camPos.x > -maxX) { // left
            camPos.x -= speed * Time.deltaTime;
        }
        if (tempY > 90 && camPos.y < maxY) { // up
            camPos.y += speed * Time.deltaTime;
        }
        if (tempY < 10 && camPos.y > -maxY) { // down
            camPos.y -= speed * Time.deltaTime;
        }
        transform.position = camPos;
    }
}
