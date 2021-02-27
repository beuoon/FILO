using System;
using System.Collections.Generic;
using UnityEngine;

public class INO_CircuitBreaker : InteractiveObject {

	public override bool IsAvailable() {
		return true;
	}

	public override void Activate() {
        if (!IsAvailable()) return;
        base.Activate();

        Debug.Log("누전차단기 작동");

        INO_Socket socket = (INO_Socket)TileMgr.Instance.GetInteractiveObject(TileMgr.GetSocketPos(position));
        if (socket != null)
            socket.Activate();
    }
}
