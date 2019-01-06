using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//skripta nastavi sorting order, da se text izpise pred vsemi tili

[ExecuteInEditMode]
[RequireComponent(typeof(TextMesh))]
public class ChangeSortingLayer : MonoBehaviour {
    public string sortingLayerName = "Default";
    public int orderInLayer = 4;

#if UNITY_EDITOR
    void Update () {
        gameObject.GetComponent<MeshRenderer>().sortingLayerName = sortingLayerName;
        gameObject.GetComponent<MeshRenderer>().sortingOrder = orderInLayer;
    }
#endif
}
