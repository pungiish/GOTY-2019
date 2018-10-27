using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour {
    public float speed;
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

        if (tempX > 90) { // right
            camPos.x += speed * Time.deltaTime;
        }
        if (tempX < 10) { // left
            camPos.x -= speed * Time.deltaTime;
        }
        if (tempY > 90) { // up
            camPos.y += speed * Time.deltaTime;
        }
        if (tempY < 10) { // down
            camPos.y -= speed * Time.deltaTime;
        }
        transform.position = camPos;
    }
}
