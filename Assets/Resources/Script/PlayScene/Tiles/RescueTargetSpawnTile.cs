using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RescueTargetSpawnTile : Tile
{
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if (go != null)
        {
            RescueTarget rt = go.GetComponent<RescueTarget>();
            if (!TileMgr.Instance.RescueTargets.ContainsKey(position))
                TileMgr.Instance.RescueTargets.Add(position, rt);
        }

        return base.StartUp(position, tilemap, go);
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/RescueTargetSpawnTile")]
    public static void CreateElectricTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save RescueTargetSpawnTile", "New RescueTargetSpawnTile", "asset", "Save RescueTargetSpawnTile", "Assets");
        if (path == "")
        {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<RescueTargetSpawnTile>(), path);
    }
#endif
}
