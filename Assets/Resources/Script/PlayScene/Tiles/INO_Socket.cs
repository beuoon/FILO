using UnityEngine;
using UnityEngine.Tilemaps;

public class INO_Socket : InteractiveObject {
    public TileBase electricTile;

    public override bool IsAvailable() {
        return false;
    }

    public override void Activate() {
        base.Activate();

        if (TileMgr.Instance.Electrics.ContainsKey(position))
            GameMgr.Instance.Obstacle.SetTile(position, null);
        else
            GameMgr.Instance.Obstacle.SetTile(position, electricTile);
    }
}
