using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GameData
{
    public const int NumOfDifferentTribes = 3;
    public const int NumOfDifferentUnits = 4;
    public const int NumOfDifferentTerrainTypes = 5;
    public const int INF = Int32.MaxValue / 2; //da ne pride do overflowa
    public const int INF_WEIGHT = INF - 1;
    public static readonly Vector3 PanelInitial = new Vector3(0, 0, 5);

    public static int numOfPlayers;
    public static GameObject[][] UnitPrefabs { get; private set; }
    public static GameObject SelectedUnitPanel { get; private set; }
    public static GameObject ReservedSpacePanel { get; private set; }
    public static GameObject HealthBar { get; private set; }

    public enum UnitType { LightMelee = 0, HeavyMelee = 1, Ranged = 2, Hero = 3};
    public static readonly UnitType[] UnitCreationSequence = 
        { UnitType.LightMelee, UnitType.LightMelee, UnitType.LightMelee, UnitType.HeavyMelee, UnitType.HeavyMelee,
        UnitType.Ranged, UnitType.Ranged, UnitType.Hero};

    public static void Init(int _numOfPlayers, UnitPrefabs prefabs, GameObject _healthBar)
    {
        numOfPlayers = _numOfPlayers;
        UnitPrefabs = new GameObject[NumOfDifferentTribes][];
        
        UnitPrefabs[0] = prefabs.Tribe0;
        UnitPrefabs[1] = prefabs.Tribe1;
        UnitPrefabs[2] = prefabs.Tribe2;

        SelectedUnitPanel = GameObject.Instantiate(prefabs.selectedUnitPanel);
        SelectedUnitPanel.transform.position = PanelInitial;

        ReservedSpacePanel = GameObject.Instantiate(prefabs.ReservedSpacePanel);
        SelectedUnitPanel.transform.position = PanelInitial;

        HealthBar = _healthBar;
    }

    public static int getIndex(int Tribe, UnitType type)
    {
        int iType = (int)type;

        return Tribe * NumOfDifferentUnits + iType;
    }

    public static readonly int[,] MoveWeights = {
        //ground, water, forest, mountain, border
        {1, 5, 4, 6, INF_WEIGHT }, //light melee 1
        {2, 15, 10, INF_WEIGHT, INF_WEIGHT }, //heavy melee 1
        {1, 7, 3, 8,  INF_WEIGHT}, //ranged unit 1
        {1, 4, 4, 4, INF_WEIGHT } //HERO 1
    };

    public static readonly int[] BaseMoveRanges = new int[] {
        15,  //light melee 1
        14,  //heavy melee 1
        15,  //ranged unit 1
        20   //HERO 1
    };

    public static readonly int[] BaseMelee = new int[]
    {
        30,     //light melee 1
        90,     //heavy melee 1
        15,     //ranged unit 1
        120     //HERO 1
    };      
            
    public static readonly int[] Health = new int[]
    {
        150, //light melee 1
        250, //heavy melee 1
        100, //ranged unit 1
        300  //HERO 1
    };

    public static readonly int[] BaseRanged = new int[]
    {
        0,  //light melee 1
        30, //heavy melee 1
        0,  //ranged unit 1
        10  //HERO 1
    };
    public static readonly int[] FightRange = new int[]
    {
        0,  //light melee 1
        10, //heavy melee 1
        0,  //ranged unit 1
        4   //HERO 1
    };
    public static readonly int[] Defence = new int[]
    {
        8,   //light melee 1
        15,  //heavy melee 1
        5,   //ranged unit 1
        20   //HERO 1
    };
}
