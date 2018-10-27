using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class MapGenerator : object {
    public static GameObject[] groundTextures;
    public static GameObject[] waterTextures;
    public static GameObject[] forestTextures;
    public static GameObject[] mountainTextures;

    public static void generateMap(ref Tilemap map) {
        // map generation
    }

    private static void createGround(int x, int y, ref Tilemap map) {
        // ground generation
    }

    private static void createWater(int x, int y, ref Tilemap map) {
        // water generation
    }

    private static void createForest(int x, int y, ref Tilemap map) {
        // forest generation
    }

    private static void createMountain(int x, int y, ref Tilemap map) {
        // mountain generation
    }
}
