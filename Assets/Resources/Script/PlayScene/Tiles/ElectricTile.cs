using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ElectricTile : Tile {
    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) {
        if (go != null) {
            Electric electric = go.GetComponent<Electric>();
            electric.position = position;
            if (!TileMgr.Instance.Electrics.ContainsKey(position))
                TileMgr.Instance.Electrics.Add(position, electric);
        }

        return base.StartUp(position, tilemap, go);
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/ElectricTile")]
    public static void CreateElectricTile() {
        string path = EditorUtility.SaveFilePanelInProject("Save ElectricTile", "New ElectricTile", "asset", "Save ElectricTile", "Assets");
        if (path == "") {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<ElectricTile>(), path);
    }
#endif
}
