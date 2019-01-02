/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class IngameObject : MonoBehaviour {
    public enum ObjectType { Unit, Building, BaseClass };
    public virtual ObjectType Type { get { return ObjectType.BaseClass; } }

    public int HealthPoints { get; private set; }
    public int Defence { get; private set; }

   

	// Use this for initialization
	void Start () {
        // GameObject.Instantiate<>()
        // Get call stack
        StackTrace stackTrace = new StackTrace();

        // Get calling method name
        UnityEngine.Debug.Log("StartIngameObject");
	}
	
    public void Click()
    {
        if(Type == ObjectType.Unit)
        {
            UnityEngine.Debug.Log("UNIT");
        }
        else
        {
            UnityEngine.Debug.Log("Ni unit");
        }
        
    }

	// Update is called once per frame
	void Update () {
		
	}
}

*/