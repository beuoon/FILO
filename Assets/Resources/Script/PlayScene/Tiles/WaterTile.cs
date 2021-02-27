using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaterTile : Tile {
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) {
        if (go != null) {
            Water water = go.GetComponent<Water>();
            water.position = position;
            if (!TileMgr.Instance.Waters.ContainsKey(position))
                TileMgr.Instance.Waters.Add(position, water);
        }

        return base.StartUp(position, tilemap, go);
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/WaterTile")]
    public static void CreateElectricTile() {
        string path = EditorUtility.SaveFilePanelInProject("Save WaterTile", "New WaterTile", "asset", "Save WaterTile", "Assets");
        if (path == "") {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<WaterTile>(), path);
    }
#endif
}
