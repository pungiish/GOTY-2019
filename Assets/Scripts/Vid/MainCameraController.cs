using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour {
    public float speed;
    public int maxX;
    public int maxY;
    private int screenWidth;
    private int screenHeight;

	void Start () {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
	}
	
	void Update () {
        Vector3 camPos = transform.position;
        int tempX = (int)((Input.mousePosition.x / Screen.width) * 100);
        int tempY = (int)((Input.mousePosition.y / Screen.height) * 100);
        // Debug.Log(tempX + " " + tempY);

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
