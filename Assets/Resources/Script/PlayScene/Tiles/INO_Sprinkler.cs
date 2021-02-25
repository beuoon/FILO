using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class INO_Sprinkler : InteractiveObject {
    public override void Activate() {
        if (!IsAvailable()) return;
        base.Activate();

        List<Fire> fires = TileMgr.Instance.Fires;
        int count = fires.Count / 2;

        for (int i = 0; i < count; i++) {
            int index = Random.Range(0, fires.Count);
            fires.RemoveAt(index);
		}
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/INO_Sprinkler")]
    public static void CreateSprinkler() {
        string path = EditorUtility.SaveFilePanelInProject("Save Sprinkler", "New Sprinkler", "asset", "Save Sprinkler", "Assets");
        if (path == "") {
            return;
        }
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<INO_Sprinkler>(), path);
    }
#endif
}
