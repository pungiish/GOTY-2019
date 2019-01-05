﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class GameData
{
    public const int NumOfDifferentTribes = 1;
    public const int NumOfDifferentUnits = 1;
    public const int NumOfDifferentTerrainTypes = 5;
    public const int INF = Int32.MaxValue / 2; //da ne pride do overflowa
    public const int INF_WEIGHT = INF - 1;

    public static int numOfPlayers;
    public static GameObject[,] UnitPrefabs;

    public static void Init(int _numOfPlayers, UnitPrefabs prefabs)
    {
        numOfPlayers = _numOfPlayers;
        UnitPrefabs = new GameObject[NumOfDifferentTribes, NumOfDifferentUnits];

        UnitPrefabs[0, 0] = prefabs.yellowUnit;
    }

    public static readonly int[,] MoveWeights = {
        //ground, water, forest, mountain, border
        {2, 15, 10, INF_WEIGHT, INF_WEIGHT }, //HULK
        {1, 7, 3, 8,  INF_WEIGHT}//Robin hood
    };

    public static readonly int[] BaseMoveRanges = new int[] {
        15, //Hulka
        20  //Robin Hood
    };

    public static readonly int[] BaseMelee = new int[]
    {
        30, //Hulk
        15 //Robin Hood
    };

    public static readonly int[] Health = new int[]
    {
        150, //Hulk
        100 //Robin Hood
    };

    public static readonly int[] BaseRanged = new int[]
    {
        0, //Hulk
        30 //RobinHood
    };
    public static readonly int[] FightRange = new int[]
    {
        0, //Hulk
        10 //RobinHood
    };
    public static readonly int[] Defence = new int[]
    {
        10, //Hulk
        1 //robinHood
    };
}