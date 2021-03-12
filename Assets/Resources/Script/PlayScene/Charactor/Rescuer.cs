using UnityEngine;

public class Rescuer : Player
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

    public override void ActiveSkill()
    {
        base.ActiveSkill();
        int range = 5;
        Vector3Int nPos = GameMgr.Instance.RescueTilemap.WorldToCell(transform.position);
        for(int i=0; i<range;i++)
        {
            for(int j=0; j<range; j++)
            {
                if(GameMgr.Instance.RescueTilemap.GetTile(nPos + new Vector3Int(i, j, 0)) != null)
                {
                    int RescueLayer = 1 << LayerMask.NameToLayer("Rescue"); // 생존자의 Layer
                    RaycastHit2D hit = Physics2D.Raycast(GameMgr.Instance.RescueTilemap.CellToWorld(nPos + new Vector3Int(i, j, 0)), Vector3.back, range, RescueLayer); // 레이캐스트 쏘기
                    if (hit)
                    {
                        if (hit.transform.CompareTag("RescueTarget")) // 레이캐스트 충돌 대상이 구조대상이라면
                        {
                        }
                    }
                }
            }
        }
    }

    protected override void Move()
    {
        base.Move();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
    }
}