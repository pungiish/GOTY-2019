using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class PlayerController : MonoBehaviour {
    //public string PlayerName { get; private set; }
    public enum Tribe { A, B, C, Undefined };
    
    public string PlayerName { get; private set; }
    public Tribe PlayerTribe { get; private set; }

    private List<Unit> Units = new List<Unit>();
    public Tilemap Map { get; private set; }
    public Tilemap HighlightMap { get; private set; }
    
    public LineRendererHandler LineHandler { get; private set; }

    public PlayerController Init(System.String playerName, MapController map, Tribe playerTribe, LineRendererHandler lh)
    {
        this.PlayerName = playerName;
        this.PlayerTribe = playerTribe;
        Map = map.map;
        HighlightMap = map.highlight;
        LineHandler = lh;

        return this;
    }
    
	void Start () {}

    public void StartTurn()
    {
        UnitHelpFunctions.PathFinding.Clear(HighlightMap, HighlightTile.TileColor.border, true);
    }
    public Unit AddNewUnit(GameData.UnitType type, Vector3Int pos)
    {
        Unit u = Instantiate(GameData.UnitPrefabs[0][(int)type]).GetComponent<Unit>();
        u.Init(this, Map.GetCellCenterWorld(pos), Map.GetTile<GameTile>(pos));
        Units.Add(u);
        Map.GetTile<GameTile>(pos).setInGameObject(u.gameObject);
        
        return u;
    }

    public void MoveUnit(Unit u, Vector3Int newPos)
    {
        u.Move(newPos);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
