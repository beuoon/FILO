using System.Collections.Generic;
using UnityEngine;

public class TileMgr // 하이어라키에 존재하지 않는 싱글톤이라 Unity 기능을 사용못합니다잉
{
    private static TileMgr m_instance; // Singleton
    public List<Fire> Fires; // 현재 생성된 Fire Instance의 Components
    public List<Wall> Walls; // 현재 생성된 Wall Instance의 Components
    public Dictionary<Vector3Int, Electric> Electrics;
    public Dictionary<Vector3Int, Water> Waters;
    
    public static TileMgr Instance
    {
        get {
            if(m_instance == null)
                m_instance = new TileMgr();
            return m_instance;
        }
    }

    public TileMgr() {
        Fires = new List<Fire>();
        Walls = new List<Wall>();
        Electrics = new Dictionary<Vector3Int, Electric>();
        Waters = new Dictionary<Vector3Int, Water>();
    }
}
