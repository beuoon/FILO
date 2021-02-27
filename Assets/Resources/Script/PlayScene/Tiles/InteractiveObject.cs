using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    protected Vector3Int _position;
    public Vector3Int position {
        set { _position = value; }
        get { return _position; }
	}
    private bool IsBeenUsed = false;

	private void OnDestroy() {
        TileMgr.Instance.SetInteractiveObject(position, null);
    }

	protected int GetAroundPlayerCount() {
        int aroundPlayerCount = 0;
        Player[] players = GameMgr.Instance.Comp_Players;
        foreach (Player player in players) {
            if ((player.currentTilePos - position).magnitude < 2)
                aroundPlayerCount++;
        }
        return aroundPlayerCount;
    }
    public virtual bool IsAvailable() {
        if (IsBeenUsed) return false;
        return true;
    }

    public virtual void Activate() {
        if (!IsAvailable()) return;
        IsBeenUsed = true;
    }
}
