using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps; // Tile Asset 생성용

public class WallTile : Tile
{
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (go != null)
        {
            go.name = "Wall(" + position.x.ToString() + ", " + position.y.ToString() + ")"; // 이름 변경(좌표값)
            Wall Comp_Wall = go.GetComponent<Wall>();
            TileMgr.Instance.Walls.Add(Comp_Wall);
        }
        return base.StartUp(position, tilemap, go);
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/WallTile")]
    public static void CreateFireTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Walltile", "New Walltile", "asset", "Save Walltile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WallTile>(), path);
    }
#endif
}
