using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour {
    public Transform bar;
    private float barRed = 0.3f;

    // call this when player health change
    // arguments: 
    // float sizeNormalized (health between 0 and 1)

    public void setHealth(float sizeNormalized) {
        if (sizeNormalized < 0.0f || sizeNormalized > 1.0f) {
            return;
        }

        bar.localScale = new Vector3(sizeNormalized, 1.0f, 1.0f);

        if (sizeNormalized <= barRed) {
            setColor(Color.red); // low health is red
        } else {
            setColor(Color.white); // normal health is white
        }
    }

    private void setColor(Color _color) {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = _color;
    }
}
