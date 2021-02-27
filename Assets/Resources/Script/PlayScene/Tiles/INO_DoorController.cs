using System;
using UnityEditor;
using UnityEngine;

public class INO_DoorController : InteractiveObject {
	public override void Activate() {
        if (!IsAvailable()) return;
        base.Activate();

        Debug.Log("컨트롤러 작동");

        INO_Door door = (INO_Door)TileMgr.Instance.GetInteractiveObject(TileMgr.GetDoorPos(position));
        if (door != null)
            door.Activate();
    }
}
