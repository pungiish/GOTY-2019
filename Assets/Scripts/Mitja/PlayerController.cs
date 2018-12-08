using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class PlayerController : MonoBehaviour {
    //public string PlayerName { get; private set; }
    public enum Tribe { A, B, C, Undefined };
    public enum UnitType { LightMelee, HeavyMelee, Ranged };

    public Tribe PlayerTribe { get; private set; }

    private List<Unit> units;
    private List<Building> buildings;
    
    
	void Start () {
        
    }

    public void AddNewUnit()
    {
    }

	// Update is called once per frame
	void Update () {
		
	}
}
