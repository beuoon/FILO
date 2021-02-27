using UnityEngine;
using UnityEngine.Tilemaps;

public class INO_Socket : InteractiveObject {
    public TileBase electricTile;

    public override bool IsAvailable() {
        return false;
    }

    public override void Activate() {
        base.Activate();

        Debug.Log("소켓 작동");
        if (TileMgr.Instance.Electrics.ContainsKey(position)) {
            GameMgr.Instance.Obstacle.SetTile(position, null);
            Debug.Log("전기 제거");
        }
        else {
            Debug.Log("전기 생성");
            GameMgr.Instance.Obstacle.SetTile(position, electricTile);
        }
    }
}
