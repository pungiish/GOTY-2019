using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {

    private void OnMouseOver() {
        Debug.Log(this.gameObject.transform.position);
    }
}
