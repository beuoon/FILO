using System.Collections;
using UnityEngine;

public class HammerMan : Player
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Move()
    {
        base.Move();
    }

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        if(_currento2 > 10) // 현재 산소가 10 이상있다면
        {
            StartCoroutine(RescueHammer()); // 스킬 발동
        }
    }

    IEnumerator RescueHammer()
    {
        Vector3Int oPos = Vector3Int.zero; // 갱신용 old Pos
        while (true) // 클릭 작용시까지 반복
        { 
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - GameMgr.Instance.BackTile.CellToWorld(currentTilePos); // 마우스 로컬 좌표
            Vector3Int direction; // 캐릭터 기준 마우스 방향
            if (Mathf.Abs(mousePos.x) > Mathf.Abs(mousePos.y))
                direction = (mousePos.x > 0) ? Vector3Int.right : Vector3Int.left;
            else
                direction = (mousePos.y > 0) ? Vector3Int.up: Vector3Int.down;
            
            Vector3Int nPos = currentTilePos + direction; // 새 좌표 갱신
            if (nPos != oPos) // 기존의 렌더부분과 갱신된 부분이 다르면
            {
                GameMgr.Instance.BackTile.SetTileFlags(oPos, UnityEngine.Tilemaps.TileFlags.None); // 기존의 좌표 색 복구
                GameMgr.Instance.BackTile.SetColor(oPos, new Color(1, 1, 1, 1));
                GameMgr.Instance.BackTile.SetTileFlags(nPos, UnityEngine.Tilemaps.TileFlags.None); // 새로운 좌표 색 변경
                GameMgr.Instance.BackTile.SetColor(nPos, new Color(0, 0, 1, 1));
                oPos = nPos;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (GameMgr.Instance.Obstacle.GetTile(oPos) != null) // 클릭 좌표에 장애물이 있다면 제거
                {
                    GameMgr.Instance.BackTile.SetTileFlags(oPos, UnityEngine.Tilemaps.TileFlags.None);
                    GameMgr.Instance.BackTile.SetColor(oPos, new Color(1, 1, 1, 1));
                    GameMgr.Instance.Obstacle.SetTile(oPos, null);
                    _currento2 -= 10;
                    O2Gage.fillAmount = _currento2 / _maxo2; // 산소 UI 변화
                }
                break;
            }
            else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                GameMgr.Instance.BackTile.SetTileFlags(oPos, UnityEngine.Tilemaps.TileFlags.None);
                GameMgr.Instance.BackTile.SetColor(oPos, new Color(1, 1, 1, 1));
                break;
            }
            yield return null;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}