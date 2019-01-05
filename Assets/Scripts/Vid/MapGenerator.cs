using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class MapGenerator : object {
    private enum LastLocation { BEG, LT, RT, LB, RB };
    
    private int maxX;
    private int maxY;
    private int mapIndex;
    private int minN, maxN;
    private Tilemap map;
    private Tilemap highlightMap;
    private Tilemap selectMap;
    private MapController mapController;
    private static int[] directions = new int[16] { 0, -1, 1, 0, 0, 1, 0, -1, -1, -1, 1, -1, 1, 1, -1, 1 };
    private static System.Random randomShuffle = new System.Random();
    private List<PlayerController> players;

    public MapGenerator(int w, int h, int i, ref Tilemap tilemap, ref Tilemap highlight, ref Tilemap selected, MapController mapC,
        List<PlayerController> playerList) {
        maxX = w / 2;
        maxY = h / 2;
        mapIndex = i;
        minN = Math.Max((w * h) / 500, 1);
        maxN = Math.Max((w * h) / 250, 1);
        map = tilemap;
        highlightMap = highlight;
        selectMap = selected;
        mapController = mapC;
        this.players = playerList;
    }

    private LastLocation lastLocation(int x, int y) {
        if (x < 0 && y >= 0) return LastLocation.LT;
        else if (x < 0 && y < 0) return LastLocation.LB;
        else if (x >= 0 && y < 0) return LastLocation.RB;
        else return LastLocation.RT;
    }

    public void generateMap() {
        Vector3Int position = new Vector3Int(0, 0, 0);
        GameObject selected;

        for (int i = -maxX; i <= maxX; i++) {
            for (int j = -maxY; j <= maxY; j++) {
                position.x = i;
                position.y = j;

                highlightMap.SetTile(position, ScriptableObject.CreateInstance<HighlightTile>().init(i, j, HighlightTile.TileColor.border, ref mapController.highlightTexture));
                selected = GameObject.Instantiate(mapController.selected);
                selected.transform.position += position;
                selected.GetComponent<MouseController>().mapController = mapController;
            }
        }

        // water
        createWater();

        // forest
        createForest();

        // mountain
        createMountains();

        // ground
        createGround();

        generateObjects();
    }

    private void generateObjects() {
        setWarriorsAndBuildings();
    }

    private void buildBase(int player, Vector3Int position) {
        GameObject gameObject = GameObject.Instantiate(mapController.buildings[GameController.selectedWarriorAndBuilding[player]]); // potrebno se nastaviti edino kateremu igralcu pripada
        gameObject.transform.position += position;
        (map.GetTile(position) as GameTile).setInGameObject(gameObject);

        Shuffle(directions, directions.Length / 2, directions.Length);
        for (int i = 0; i < GameData.UnitCreationSequence.Length; ++i) {
            position.x += directions[2 * i]; position.y += directions[2 * i + 1];

            GameObject gameObject2 = players[player].AddNewUnit(GameData.UnitCreationSequence[i], position).gameObject; 
            (map.GetTile(position) as GameTile).setInGameObject(gameObject2);
            
            position.x -= directions[2 * i]; position.y -= directions[2 * i + 1];
        }
    }

    private void setWarriorsAndBuildings() {
        int[] boundaries = { -maxX, 0, -maxY, 0, 0, maxX, 0, maxY, -maxX, 0, 0, maxY, 0, maxX, -maxY, 0  };
        int x1, x2, y1, y2, randX, randY, offset = maxX / 3, counter;
        Vector3Int position = Vector3Int.zero;

        // tukaj bo potrebno spremeniti, da se bodo ustvarjali objekti tipa Warrior in bodo pripadali dolocenemu igralcu
        for (counter = 0; counter < GameController.numberOfPlayers; counter++) {
            x1 = boundaries[(counter % 4)*4]; x2 = boundaries[(counter % 4) * 4 + 1];
            y1 = boundaries[(counter % 4) * 4 + 2]; y2 = boundaries[(counter % 4) * 4 + 3];
            
            while (true) {
                randX = UnityEngine.Random.Range(x1 + offset, x2 - offset);
                randY = UnityEngine.Random.Range(y1 + offset, y2 - offset);
                position.x = randX; position.y = randY;

                if ((map.GetTile(position) as GameTile).getType() != GameTile.TileType.water) {
                    break;
                }
            }
            
            buildBase(counter, position);
        }

        for (int i = 0; i < 3; i++) {
            if (counter < 4) {
                x1 = boundaries[(counter % 4) * 4]; x2 = boundaries[(counter % 4) * 4 + 1];
                y1 = boundaries[(counter % 4) * 4 + 2]; y2 = boundaries[(counter % 4) * 4 + 3];

                while (true) {
                    randX = UnityEngine.Random.Range(x1 + offset, x2 - offset);
                    randY = UnityEngine.Random.Range(y1 + offset, y2 - offset);
                    position.x = randX; position.y = randY;

                    if ((map.GetTile(position) as GameTile).getType() != GameTile.TileType.water) {
                        break;
                    }
                }

                GameObject gameObject = GameObject.Instantiate(mapController.buildings[mapController.buildings.Length - 1]); // potrebno se nastaviti edino kateremu igralcu pripada
                gameObject.transform.position += position;
                (map.GetTile(position) as GameTile).setInGameObject(gameObject);

                counter++;
            } else {
                x1 = 0; x2 = 0; y1 = 0; y2 = 0;

                while (true) {
                    randX = UnityEngine.Random.Range(x1 - offset, x2 + offset);
                    randY = UnityEngine.Random.Range(y1 - offset, y2 + offset);
                    position.x = randX; position.y = randY;

                    if ((map.GetTile(position) as GameTile).getType() != GameTile.TileType.water) {
                        break;
                    }
                }

                GameObject gameObject = GameObject.Instantiate(mapController.buildings[mapController.buildings.Length - 1]); // potrebno se nastaviti edino kateremu igralcu pripada
                gameObject.transform.position += position;
                (map.GetTile(position) as GameTile).setInGameObject(gameObject);

                counter++;
            }
        }
    }

    private bool outOfMap(Vector3Int position) {
        return position.x < -maxX || position.x > maxX || position.y < -maxY || position.y > maxY;
    }

    private bool setTile(Vector3Int position, GameTile.TileType type, ref Sprite texture) {
        if (!map.HasTile(position)) {
            map.SetTile(position, ScriptableObject.CreateInstance<GameTile>().init(position.x, position.y, type, ref texture));
            return true;
        }
        return false;
    }

    private void createGround() {
        Vector3Int position = new Vector3Int(0, 0, 0);
        bool afterMountainTile = true, insideMoutain;

        for (int i = -maxX; i <= maxX; i++) {
            for (int j = -maxY; j <= maxY; j++) {
                position.x = i;
                position.y = j;

                if (map.HasTile(position)) {
                    afterMountainTile = mapController.getTileAt(position).getType() == GameTile.TileType.mountain;
                    continue;
                }

                if (afterMountainTile) {
                    insideMoutain = true;

                    for (int k = 0; k < directions.Length / 2; k += 2) {
                        position.x += directions[k];
                        position.y += directions[k + 1];

                        if (!outOfMap(position) && (!map.HasTile(position) || (mapController.getTileAt(position).getType() != GameTile.TileType.mountain))) {
                            insideMoutain = false;
                        }

                        position.x -= directions[k];
                        position.y -= directions[k + 1];
                    }

                    if (insideMoutain) {
                        map.SetTile(position, ScriptableObject.CreateInstance<GameTile>().init(position.x, position.y, GameTile.TileType.mountain, ref mapController.mountainTextures[mapIndex]));
                    }
                    else {
                        map.SetTile(position, ScriptableObject.CreateInstance<GameTile>().init(position.x, position.y, GameTile.TileType.ground, ref mapController.groundTextures[mapIndex]));
                        afterMountainTile = false;
                    }
                } else {
                    map.SetTile(position, ScriptableObject.CreateInstance<GameTile>().init(position.x, position.y, GameTile.TileType.ground, ref mapController.groundTextures[mapIndex]));
                }
            }
        }
    }

    private void createWater() {
        int leftSideY = UnityEngine.Random.Range(-maxY + 2, maxY - 2);
        int rightSideY = UnityEngine.Random.Range(-maxY + 2, maxY - 2);
        int topSideX = UnityEngine.Random.Range(-maxX + 2, maxX - 2);
        int bottomSideX = UnityEngine.Random.Range(-maxX + 2, maxX - 2);

        if (UnityEngine.Random.Range(-5.0f, 4.0f) <= 0) { // left side + top side, right side + bottom side
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
        int offset; // offset
        int minX = (x1 < x2) ? x1 : x2;
        int maxX = (x1 > x2) ? x1 : x2;

        position.y = y1;
        for (int i = minX; i <= maxX; i++) {
            position.x = i;
            setTile(position, GameTile.TileType.water, ref mapController.waterTextures[mapIndex]);

            // offset
            offset = UnityEngine.Random.Range(0, 9);

            if (offset <= 2) {
                if (position.y > y1 - 1) {
                    position.y--;
                    if (!outOfMap(position)) {
                        setTile(position, GameTile.TileType.water, ref mapController.waterTextures[mapIndex]);
                    } else {
                        position.y++;
                    }
                }
            } else if (offset >= 6) {
                if (position.y < y1 + 1) {
                    position.y++;
                    if (!outOfMap(position)) {
                        setTile(position, GameTile.TileType.water, ref mapController.waterTextures[mapIndex]);
                    }
                    else {
                        position.y--;
                    }
                }
            }

            if (i == maxX) {
                position.y = y1;
                setTile(position, GameTile.TileType.water, ref mapController.waterTextures[mapIndex]);
            }
        }

        int minY = (y1 < y2) ? y1 : y2;
        int maxY = (y1 > y2) ? y1 : y2;

        position.x = x2;
        for (int i = minY; i <= maxY; i++) {
            position.y = i;
            setTile(position, GameTile.TileType.water, ref mapController.waterTextures[mapIndex]);

            // offset
            offset = UnityEngine.Random.Range(0, 9);

            if (offset <= 2) {
                if (position.x > x2 - 1) {
                    position.x--;
                    if (!outOfMap(position)) {
                        setTile(position, GameTile.TileType.water, ref mapController.waterTextures[mapIndex]);
                    }
                    else {
                        position.x++;
                    }
                }
            }
            else if (offset >= 6) {
                if (position.x < x2 + 1) {
                    position.x++;
                    if (!outOfMap(position)) {
                        setTile(position, GameTile.TileType.water, ref mapController.waterTextures[mapIndex]);
                    }
                    else {
                        position.x--;
                    }
                }
            }

            if (i == maxY) {
                position.x = x2;
                setTile(position, GameTile.TileType.water, ref mapController.waterTextures[mapIndex]);
            }
        }
    }

    private void createForest() {
        int forestCounter = UnityEngine.Random.Range(minN, maxN);
        int width, startX, startY;
        LastLocation ll = LastLocation.BEG;

        for (int i = 0; i < forestCounter; i++) {
            width = UnityEngine.Random.Range(5, 7);

            while(true) {
                startX = UnityEngine.Random.Range(-maxX + 2, maxX - 2);
                startY = UnityEngine.Random.Range(-maxY + 2, maxY - 2);

                if (lastLocation(startX, startY) != ll) {
                    ll = lastLocation(startX, startY);
                    break;
                }
            }
            
            spreadForest(startX, startY, width);
        }
    }

    private void spreadForest(int x1, int y1, int width) {
        int length = UnityEngine.Random.Range(4, 6);
        Vector3Int pos = new Vector3Int(0, 0, 0);

        for (int i = 0; i < length; i++) {
            for (int j = 0; j < width; j++) {
                pos.x = x1 - i;

                pos.y = y1 - j;
                if (!outOfMap(pos) && (j < width - 2 || UnityEngine.Random.Range(-5, 4) > 0)) setTile(pos, GameTile.TileType.forest, ref mapController.forestTextures[mapIndex]);
                pos.y = y1 + j;
                if (!outOfMap(pos) && (j < width - 2 || UnityEngine.Random.Range(-5, 4) > 0)) setTile(pos, GameTile.TileType.forest, ref mapController.forestTextures[mapIndex]);

                pos.x = x1 + i;

                if (!outOfMap(pos) && (j < width - 2 || UnityEngine.Random.Range(-5, 4) > 0)) setTile(pos, GameTile.TileType.forest, ref mapController.forestTextures[mapIndex]);
                pos.y = y1 - j;
                if (!outOfMap(pos) && (j < width - 2 || UnityEngine.Random.Range(-5, 4) > 0)) setTile(pos, GameTile.TileType.forest, ref mapController.forestTextures[mapIndex]);
            }
        }
    }

    private void createMountains() {
        int numberOfMountains = UnityEngine.Random.Range(minN, maxN);
        int counter;
        Vector3Int position = new Vector3Int(0, 0, 0);
        LastLocation ll = LastLocation.BEG;

        for (int i = 0; i < numberOfMountains; i++) {
            counter = UnityEngine.Random.Range(35, 45);

            while (true) {
                position.x = UnityEngine.Random.Range(-maxX + 2, maxX - 2);
                position.y = UnityEngine.Random.Range(-maxY + 2, maxY - 2);

                if (!map.HasTile(position)) {
                    if (lastLocation(position.x, position.y) != ll) {
                        ll = lastLocation(position.x, position.y);
                        spreadMountain(position, ref counter);
                        break;
                    }
                }
            }   
        }
    }

    private void spreadMountain(Vector3Int currentPosition, ref int counter) {
        setTile(currentPosition, GameTile.TileType.mountain, ref mapController.mountainTextures[mapIndex]);
        counter--;

        for (int i = 0; i < directions.Length / 2 && counter != 0; i += 2) {
            currentPosition.x += directions[i];
            currentPosition.y += directions[i + 1];

            if (!outOfMap(currentPosition) && !map.HasTile(currentPosition)) {
                setTile(currentPosition, GameTile.TileType.mountain, ref mapController.mountainTextures[mapIndex]);
                counter--;
            }
            
            currentPosition.x -= directions[i];
            currentPosition.y -= directions[i + 1];
        }

        Shuffle(directions, directions.Length / 2, directions.Length);

        for (int i = directions.Length - 1; i > 0 && counter != 0; i -= 2) {
            currentPosition.x += directions[i];
            currentPosition.y += directions[i - 1];

            if (!outOfMap(currentPosition) && !map.HasTile(currentPosition)) {
                spreadMountain(currentPosition, ref counter);
            }

            currentPosition.x -= directions[i];
            currentPosition.y -= directions[i - 1];
        }
    }

    private static void Shuffle(int[] array, int start, int end) {
        for (int i = start; i < end; i += 2) {
            int r = i + randomShuffle.Next(end - i);
            if (r % 2 != 0) r--;

            int t1 = array[r];
            int t2 = array[r + 1];
            array[r] = array[i];
            array[r + 1] = array[i + 1];
            array[i] = t1;
            array[i + 1] = t2;
        }
    }
}