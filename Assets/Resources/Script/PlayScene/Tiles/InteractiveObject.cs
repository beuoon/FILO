using UnityEngine;
using UnityEngine.Tilemaps; // Tile Asset 생성용

public abstract class InteractiveObject : Tile
{
    public const string TILE_NAME = "InteractiveObject";
    private bool IsBeenUsed;
    protected Vector3Int Position;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go) {
        Position = position;
        name = TILE_NAME;
        IsBeenUsed = false;

        return base.StartUp(position, tilemap, go);
    }

    public virtual bool IsAvailable() {
        if (IsBeenUsed) return false;

        int aroundPlayerCount = 0;
        Player[] players = GameMgr.Instance.Comp_Players;
        foreach (Player player in players) {
            if ((player.currentTilePos - Position).magnitude < 2)
                aroundPlayerCount++;
        }

        if (aroundPlayerCount < 1)
            return false;
        return true;
    }

    public virtual void Activate() {
        if (!IsAvailable()) return;
        IsBeenUsed = true;
	}
}
