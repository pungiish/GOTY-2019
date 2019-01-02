using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlayerController : MonoBehaviour {
    //public string PlayerName { get; private set; }
    public enum Tribe { A, B, C, Undefined };
    public enum UnitType { LightMelee, HeavyMelee, Ranged, Hero };
    
    public string PlayerName { get; private set; }
    public Tribe PlayerTribe { get; private set; }

    private List<Unit> Units = new List<Unit>();
    public Tilemap Map { get; private set; }
    public Tilemap HighlightMap { get; private set; }

    private UnitPrefabs Prefabs;
    public LineRendererHandler LineHandler { get; private set; }

    public PlayerController Init(System.String playerName, MapController map, Tribe playerTribe, UnitPrefabs prefabs, LineRendererHandler lh)
    {
        this.PlayerName = playerName;
        this.PlayerTribe = playerTribe;
        Map = map.map;
        HighlightMap = map.highlight;
        Prefabs = prefabs;
        LineHandler = lh;

        return this;
    }
    
	void Start () {
        
    }

    public Unit AddNewUnit(UnitType type, Vector3Int pos)
    {
        Unit u = Instantiate(Prefabs.yellowUnit).GetComponent<Unit>();
        u.Init(this, Map.GetCellCenterWorld(pos), Map.GetTile<GameTile>(pos));
        Units.Add(u);
        Map.GetTile<GameTile>(pos).setInGameObject(u.gameObject);
        
        /*
        u.ShowPossibleMoves();
        u.DrawMoveLineToDest(new Vector3Int(0, 3, 0));
        u.Move(new Vector3Int(0, 3, 0));
        */
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
