using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour {
    public Transform bar;
    private float barLow = 0.3f;
    private Color normalHealth = Color.white;
    private Color lowHealth = Color.red;

    private float MaxHealth = 1.0f;

    // call this when player health change
    // arguments: 
    // float sizeNormalized (health between 0 and 1)

    public void setHealthBarColors(Color NormalHealth, Color LowHealth)
    {
        normalHealth = NormalHealth;
        lowHealth = LowHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        MaxHealth = maxHealth;
    }

    public void setHealth(float size) {
        if(MaxHealth != 1.0f)
            size /= MaxHealth;

        if (size < 0.0f || size > 1.0f) {
            return;
        }

        bar.localScale = new Vector3(size, 1.0f, 1.0f);

        if (size <= barLow) {
            setColor(lowHealth); // low health is red
        } else {
            setColor(normalHealth); // normal health is white
        }
    }

    private void setColor(Color _color) {
        bar.Find("BarSprite").GetComponent<SpriteRenderer>().color = _color;
    }
}
