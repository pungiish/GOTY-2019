using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : object {
    private int maxX;
    private int maxY;
    private int mapIndex;
    private Tilemap map;
    private Tilemap highlightMap;
    private MapController mapController;

    public MapGenerator(int w, int h, int i, ref Tilemap tilemap, ref Tilemap highlight, MapController mapC) {
        maxX = w / 2;
        maxY = h / 2;
        mapIndex = i;
        map = tilemap;
        highlightMap = highlight;
        mapController = mapC;
    }

    public void generateMap() {
        Vector3Int position = new Vector3Int(0, 0, 0);

        for (int i = -maxX; i <= maxX; i++) {
            for (int j = -maxY; j <= maxY; j++) {
                position.x = i;
                position.y = j;

                highlightMap.SetTile(position, ScriptableObject.CreateInstance<GameTile>().init(i, j, GameTile.TileType.ground, ref mapController.highlightTexture));
            }
        }

        // water
        createWater();

        // forest
        createForest();

        // mountain
        createMountain();
    }

    private void createGround(int x, int y, ref Tilemap map) {
        // ground generation
    }

    private void createWater() {
        int leftSideY = Random.Range(-maxY + 2, maxY - 2);
        int rightSideY = Random.Range(-maxY + 2, maxY - 2);
        int topSideX = Random.Range(-maxX + 2, maxX - 2);
        int bottomSideX = Random.Range(-maxX + 2, maxX - 2);

        if (Random.Range(-5.0f, 4.0f) <= 0) { // left side + top side, right side + bottom side
            if (topSideX > 0) topSideX *= -1;
            if (bottomSideX < 0) bottomSideX *= -1;

            connectWaterPoint(-maxX, leftSideY, topSideX, maxY);
            connectWaterPoint(maxX, rightSideY, bottomSideX, -maxY);
        } else { // left side + bottom side, right side + top size
            if (topSideX < 0) topSideX *= -1;
            if (bottomSideX > 0) bottomSideX *= -1;

            connectWaterPoint(-maxX, leftSideY, bottomSideX, -maxY);
            connectWaterPoint(maxX, rightSideY, topSideX, maxY);
        }
    }

    private void connectWaterPoint(int x1, int y1, int x2, int y2) {
        Vector3Int position = new Vector3Int(0, 0, 0);
        int minX = (x1 < x2) ? x1 : x2;
        int maxX = (x1 > x2) ? x1 : x2;

        position.y = y1;
        for (int i = minX; i <= maxX; i++) {
            position.x = i;
            setTile(position, GameTile.TileType.water, ref mapController.waterTextures[mapIndex]);
        }

        int minY = (y1 < y2) ? y1 : y2;
        int maxY = (y1 > y2) ? y1 : y2;

        position.x = x2;
        for (int i = minY; i <= maxY; i++) {
            position.y = i;
            setTile(position, GameTile.TileType.water, ref mapController.waterTextures[mapIndex]);
        }
    }

    private void createForest() {
        int forestCounter = Random.Range(5, 7);
        int width, startX, startY;

        for (int i = 0; i < forestCounter; i++) {
            width = Random.Range(4, 6);

            startX = Random.Range(-maxX + 2, maxX - 2);
            startY = Random.Range(-maxY + 2, maxY - 2);

            spreadForest(startX, startY, width);
        }
    }

    private void spreadForest(int x1, int y1, int width) {
        int length = Random.Range(4, 6);
        Vector3Int pos = new Vector3Int(0, 0, 0);

        for (int i = 0; i < length; i++) {
            for (int j = 0; j < width; j++) {
                if (x1 - i >= -maxX) {
                    pos.x = x1 - i;

                    if (y1 - j >= -maxY) {
                        pos.y = y1 - j;
                        setTile(pos, GameTile.TileType.forest, ref mapController.forestTextures[mapIndex]);
                    }

                    if (y1 + j <= maxY) {
                        pos.y = y1 + j;
                        setTile(pos, GameTile.TileType.forest, ref mapController.forestTextures[mapIndex]);
                    }

                }
                if (x1 + i <= maxX) {
                    pos.x = x1 + i;

                    if (y1 - j >= -maxY) {
                        pos.y = y1 - j;
                        setTile(pos, GameTile.TileType.forest, ref mapController.forestTextures[mapIndex]);
                    }

                    if (y1 + j <= maxY) {
                        pos.y = y1 + j;
                        setTile(pos, GameTile.TileType.forest, ref mapController.forestTextures[mapIndex]);
                    }
                }
            }
        }
    }

    private void setTile(Vector3Int position, GameTile.TileType type, ref Sprite texture) {
        if (!map.HasTile(position)) {
            map.SetTile(position, ScriptableObject.CreateInstance<GameTile>().init(position.x, position.y, type, ref texture));
        }
    }

    private void createMountain() {
        // mountain generation
    }
}
