using System.Collections.Generic;
using UnityEngine;
using System;

public class TileMgr // 하이어라키에 존재하지 않는 싱글톤이라 Unity 기능을 사용못합니다잉
{   
    private static TileMgr m_instance; // Singleton
    public List<Fire> Fires; // 현재 생성된 Fire Instance의 Components
    public List<Wall> Walls; // 현재 생성된 Wall Instance의 Components
    public Dictionary<Vector3Int, Electric> Electrics;
    public Dictionary<Vector3Int, Water> Waters;
    
    private Dictionary<Vector3Int, InteractiveObject> m_interactiveObjects;

    private static readonly List<Dictionary<Vector3Int, Vector3Int>> DoorPairs = new List<Dictionary<Vector3Int, Vector3Int>>(){
        new Dictionary<Vector3Int, Vector3Int>(){
            {new Vector3Int(-2, -4, 0), new Vector3Int(0, -4, 0)},
        }
    };
    private static readonly List<Dictionary<Vector3Int, Vector3Int>> SocketPairs = new List<Dictionary<Vector3Int, Vector3Int>>(){
        new Dictionary<Vector3Int, Vector3Int>(){
            {new Vector3Int(2, -2, 0), new Vector3Int(4, -3, 0)},
        }
    };
    public static Vector3Int GetDoorPos(Vector3Int pos) {
        return DoorPairs[GameMgr.Instance.stage][pos];
    }
    public static Vector3Int GetSocketPos(Vector3Int pos) {
        return SocketPairs[GameMgr.Instance.stage][pos];
    }

    public static TileMgr Instance {
        get {
            if (m_instance == null)
                m_instance = new TileMgr();
            return m_instance;
        }
    }

    public TileMgr() {
        Fires = new List<Fire>();
        Walls = new List<Wall>();
        Electrics = new Dictionary<Vector3Int, Electric>();
        Waters = new Dictionary<Vector3Int, Water>();

        m_interactiveObjects = new Dictionary<Vector3Int, InteractiveObject>();
    }

    public void SetInteractiveObject(Vector3Int pos, InteractiveObject obj) {
        m_interactiveObjects.Remove(pos);
        m_interactiveObjects.Add(pos, obj);
	}
    public InteractiveObject GetInteractiveObject(Vector3Int pos) {
        if (m_interactiveObjects.ContainsKey(pos))
            return m_interactiveObjects[pos];
        return null;
    }
}
