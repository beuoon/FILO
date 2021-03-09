using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps; // Tilemap용
using UnityEngine.UI; // UI 용

public class GameMgr : MonoBehaviour
{
    private static GameMgr _instance; // Singleton
    public static GameMgr Instance // 하이어라키에 존재합니다잉
    {
        get
        {
            if(_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }


    public int GameTurn = 0; // 게임 턴
    public Player[] Comp_Players; // 사용할 캐릭터들의 Components
    private int _currentChar = 0; // 현재 사용중인 캐릭터의 번호
    private float _EmberTime = 0.0f; // 작은 불 생성 주기
    private int _usedEmberCount = 0; // 현재 사용중인 작은 불의 숫자
    public Transform Embers; // 작은 불의 부모 오브젝트의 Transform
    public GameObject Ember; // 작은 불 Prefab
    public Tilemap BackTile; // Background Tilemap
    public Tilemap Obstacle; // Obstacel Tilemap
    public Tilemap RescueTilemap; // RescueTarget Tilemap

    [SerializeField]
    private Text _timerText = null; // 시계 UI
    private int _minute; // 게임 상의 시간

    private int _stage = 0;

    private EventMgr eventMgr;

    public int stage {
        get { return _stage; }
    }

    public int CurrentChar
    {
        get { return _currentChar; }
        set { _currentChar = value; }
    } // CurrentChar Property

    public float EmberTime
    {
        get { return _EmberTime; }
        set { _EmberTime = value; }
    } // 작은 불 생성 주기Property

    public int UsedEmberCount
    {
        get { return _usedEmberCount; }
        set { _usedEmberCount = value; }
    } //현재 사용중인 작은 불의 숫자 Property

    private void SetupStage(int stageNumber) {
        _stage = stageNumber;
        eventMgr = new EventMgr(stageNumber);
    }


    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        for (int i = 0; i < 100; i++) // 작은 불 생성
        {
            Instantiate(Ember, Vector3.zero, Quaternion.identity, Embers);
        }
        //for(int i=BackTile.cellBounds.xMin; i<BackTile.cellBounds.xMax; i++)
        //{
        //    for(int j=BackTile.cellBounds.yMin; j<BackTile.cellBounds.yMax; j++)
        //    {
        //        Vector3Int nPos = new Vector3Int(i, j, 0);
        //        if (BackTile.GetTile(nPos) != null)
        //        {
        //            FogTile.SetTile(nPos, BlackFog);
        //        }
        //    }
        //}
        _minute = 1189; // 게임 상의 시간
    }

	private void Start() {
        SetupStage(stage);
	}

	public bool CheckEmberTick()
    {
        if (_EmberTime > 2.0f) // 2초가 지나면 true 반환
        {
            _EmberTime = 0.0f;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TurnEnd() // 턴 종료 버튼 클릭 시 호출되는 함수
    {
        eventMgr.TurnUpdate();

        for (int i = 0; i < Comp_Players.Length; i++) // 캐릭터들의 턴 종료 행동 함수 호출
        {
            Comp_Players[i].TurnEndActive();
        }
        for(int i=0, count = TileMgr.Instance.Fires.Count; i< count; i++) // 큰 불 생성
        {
            TileMgr.Instance.Fires[i].SpreadFire();
        }
        GameTurn++; // 턴 증가
        foreach(var rt in TileMgr.Instance.RescueTargets) // 생존자 턴 종료 이동
        {
             rt.Value.TurnEndActive();
        }
        Debug.Log(GameTurn + "TurnEnd");
        _minute += 5;
        _timerText.text = (_minute / 60).ToString() + " : " + (_minute % 60).ToString(); // 시계 UI 갱신
    }
}
