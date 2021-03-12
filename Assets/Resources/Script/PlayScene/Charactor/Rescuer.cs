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
        Debug.Log("구조맨 스킬 사용");
        int range = 5;
        int offset = -(range / 2);
        Vector3Int nPos = GameMgr.Instance.RescueTilemap.WorldToCell(transform.position);
        for(int i= offset; i<range + offset;i++)
        {
            for(int j= offset; j<range + offset; j++)
            {
                if(TileMgr.Instance.RescueTargets.ContainsKey(nPos + new Vector3Int(i, j, 0)))
                {
                    TileMgr.Instance.RescueTargets[nPos + new Vector3Int(i, j, 0)].ActiveSmileMark();
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