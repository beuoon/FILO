using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class INO_Sprinkler : InteractiveObject {
    public override void Activate() {
        if (!IsAvailable()) return;
        base.Activate();

        Debug.Log("스프링쿨러 작동");

        List<Fire> fires = TileMgr.Instance.Fires;
        int count = fires.Count / 2;

        for (int i = 0; i < count; i++) {
            int index = Random.Range(0, fires.Count);
            fires.RemoveAt(index);
		}
    }
}
