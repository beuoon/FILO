using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps; // Tile Asset 생성용

public class FireTile : Tile
{
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) // Start
    {
        if(go != null)
        {
            go.name = "Fire(" + position.x.ToString() + ", " + position.y.ToString() + ")";  // 이름 설정(좌표값)
            Fire Comp_fire = go.GetComponent<Fire>();
            Comp_fire.Pos = position; // Fire Instance의 좌표값 설정
            TileMgr.Instance.Fires.Add(Comp_fire); // TileMgr에 현재 생성된 Fire 추가
        }
        return base.StartUp(position, tilemap, go);
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/FireTile")]
    public static void CreateFireTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Firetile", "New Firetile", "asset", "Save Firetile", "Assets");
        if(path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<FireTile>(), path);
    }
#endif
}
