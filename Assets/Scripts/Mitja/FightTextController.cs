using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTextController : MonoBehaviour {

	// Use this for initialization
	void Start () {}

    public void Init(string text, Vector3 position, int steps, float speed)
    {
        this.transform.position = position;
        this.GetComponent<TextMesh>().text = text;
        StartCoroutine(FadeOut(steps, speed));
    }

    private IEnumerator FadeOut(int steps, float speed)
    {
        float alphaStep = 1.0f / steps;
        float i = 1.0f;
        TextMesh mesh = this.GetComponent<TextMesh>();
        Color meshColor;

        Vector3 posStep = new Vector3(0, 0, 0);

        while (i > 0.0f)
        {
            meshColor = mesh.color;
            meshColor.a = i;
            mesh.color = meshColor;
            i -= alphaStep;

            posStep.y = speed * Time.deltaTime; // objekt se premika navzgor
            this.transform.position += posStep;
            yield return null;
        }

        Destroy(this.gameObject);
        yield break;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
