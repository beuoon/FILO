﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    // UI 및 타일 정보
    public Image O2Gage; // 산소
    public Image HPGage; // 체력
    public Image MTGage; // 멘탈
    public GameObject UI_Actives; // 행동 버튼 UI
    public GameObject UI_ToolBtns; // 도구 버튼 UI
    public GameObject FireWall; // 방화벽 Prefab
    public TileBase FireWallTile; // 타일맵에 적용할 방화벽

    // 플레이어 스테이터스
    //안녕안녕?
    protected enum _Act { Idle, Walk, Run, Rescue, Interact } // 캐릭터 행동 상태 종류
    protected _Act _playerAct; 
    private RescueTarget _rescueTarget; // 현재 구조중인 타겟
    [SerializeField]
    private int _playerNum = 0; // 캐릭터 번호
    [SerializeField]
    private float _movespeed = 0.0f; // 캐릭터 이동속도
    [SerializeField]
    private float _maxo2 = 0.0f; // 캐릭터 최대 산소량
    public float MaxO2 // O2 Property
    {
        get { return _maxo2; }
    }
    private float _currento2 = 0.0f; // 캐릭터 현재 산소량
    public float CurrentO2 // 현재 O2 Property
    {
        get { return _currento2; }
        set { _currento2 = value; }
    }
    [SerializeField]
    private float _useo2 = 0.0f; // 1초에 사용되는 산소량
    [SerializeField]
    private float _maxHp = 0.0f; // 최대 체력
    private float _currentHp = 0.0f; // 현재 체력
    [SerializeField]
    private float _maxMental = 0; // 최대 멘탈
    private float _currentMental = 0; // 현재 멘탈
    private Vector3 _moveDir = Vector3.left; // 캐릭터가 바라보는 방향

    // 타일 충돌체크용 값
    private GridLayout _tileLayout; // 타일맵 값 변경용 변수 (Tilemap::Background)
    private Vector3Int _currentTilePos = Vector3Int.zero; // 현재 캐릭터의 타일맵 좌표
    public Vector3Int currentTilePos
    {
        get { return _currentTilePos; }
    }

    // Local Component
    private Animator _anim; // 캐릭터 애니메이션
    [SerializeField]
    private Transform _body = null; // 캐릭터 이미지의 Transform

    protected virtual void Start()
    {
        _tileLayout = GameMgr.Instance.BackTile.GetComponent<GridLayout>();
        _anim = GetComponentInChildren<Animator>();
        _currento2 = _maxo2;
        _currentHp = _maxHp;
        _currentMental = _maxMental;
        //SetFOV();
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
        Activate();
    }

    protected virtual void Move()
    {
        float hor = Input.GetAxisRaw("Horizontal"); // 가속도 없이 Raw값 사용
        float ver = Input.GetAxisRaw("Vertical");

        //구조 상태가 아니며, 현재 체력과 산소가 남아있는 현재 조종중인 캐릭터를 Translate로 이동시킨다.
        if (_currento2 > 0.0f && GameMgr.Instance.CurrentChar == _playerNum && _currentHp > 0.0f && _playerAct != _Act.Rescue)
        {
            this.transform.Translate(hor * Time.deltaTime * _movespeed, ver * Time.deltaTime * _movespeed, 0.0f);
            O2Gage.fillAmount = _currento2 / _maxo2; // 산소 UI 변화
            if (GameMgr.Instance.CheckEmberTick()) // 작은 불 재생성
            {
                for (int i = 0; i < GameMgr.Instance.UsedEmberCount; i++) // 기존에 있던 작은불들 Active false
                {
                    GameMgr.Instance.Embers.GetChild(i).gameObject.SetActive(false);
                }
                GameMgr.Instance.UsedEmberCount = 0;
                for (int i = 0; i < TileMgr.Instance.Fires.Count; i++) // 현재 남아있는 불들의 수만큼 작은불 재생성
                {
                    TileMgr.Instance.Fires[i].FindEmberArea();
                }
            }
            if (hor != 0.0f) // 좌, 우 이동중이라면
            {
                _currento2 -= Time.deltaTime * _useo2; // 산소 감소
                GameMgr.Instance.EmberTime += Time.deltaTime / 2; // 작은불 재생성 시간 증가
                _moveDir = new Vector3(hor, 0, 0); // 바라보는 방향 변경
            }
            if (ver != 0.0f) //상, 하 이동중이라면
            {
                _currento2 -= Time.deltaTime * _useo2; // 산소 감소
                GameMgr.Instance.EmberTime += Time.deltaTime / 2; // 작은불 재생성 시간 증가
            }
            //if (_currentTilePos != _tileLayout.WorldToCell(transform.position))
            //{
            //    SetFOV();
            //}
            if((hor != 0 || ver != 0) && _anim.GetBool("IsRunning") == false) // 이동 시작 시
            {
                _anim.SetBool("IsRunning", true); // 달리기 애니메이션 재생
            }
            if(_moveDir.x > 0) // 바라보는 방향이 우측이라면
            {
                _body.rotation = new Quaternion(0, 180.0f, 0, this.transform.rotation.w); // 우측으로 이미지 회전
            }
            else if(_moveDir.x < 0) // 좌측이라면
            {
                _body.rotation = Quaternion.identity; // y값 초기화
            }
            _currentTilePos = _tileLayout.WorldToCell(transform.position); // 현재 캐릭터의 타일맵 좌표 갱신
        }
        if (hor == 0 && ver == 0) // 이동 종료 시
        {
            _anim.SetBool("IsRunning", false); // 달리기 애니메이션 종료
        }
    }

    protected InteractiveObject SearchAroundInteractiveObject(Vector3Int pos) {
        // 주변에 있는 상호작용 오브젝트 탐색
        for (int y = -1; y <= 1; y++) {
            for (int x = -1; x <= 1; x++) {
                Vector3Int targetPos = pos - new Vector3Int(x, y, 0);
                TileBase tile = GameMgr.Instance.Obstacle.GetTile(targetPos);

                if (tile != null && tile.name == InteractiveObject.TILE_NAME)
                    return (InteractiveObject)tile;
            }
        }

        return null;
    }
    protected void ActivateInteractBtn(InteractiveObject interactiveObject) {
        GameObject InteractiveBtn = UI_Actives.transform.Find("InteractBtn").gameObject;
        Button button = (Button)InteractiveBtn.GetComponent("Button");
        button.onClick.AddListener(interactiveObject.Activate);

        InteractiveBtn.SetActive(true);
    }
    protected void DeactivateInteractBtn() {
        GameObject InteractiveBtn = UI_Actives.transform.Find("InteractBtn").gameObject;
        InteractiveBtn.SetActive(false);
    }

    protected virtual void Activate() // 행동 들 (구조, 도구사용)
    {
        InteractiveObject interactiveObject = SearchAroundInteractiveObject(_currentTilePos);
        if (interactiveObject != null && interactiveObject.IsAvailable())
            ActivateInteractBtn(interactiveObject);
        else
            DeactivateInteractBtn();

        if (Input.GetMouseButtonUp(1) && GameMgr.Instance.CurrentChar == _playerNum) // 마우스 우클릭 시 현재 조작중인 캐릭터의 버튼 UI 표시
        {
            if (UI_Actives.activeSelf == true || UI_ToolBtns.activeSelf == true) // 이미 켜져있었다면 UI 끄기
            {
                UI_Actives.SetActive(false);
                UI_ToolBtns.SetActive(false);
            }
            else if (UI_Actives.activeSelf == false && UI_ToolBtns.activeSelf == false) // Active UI 보이기
            {
                UI_Actives.SetActive(true);
            }
        }
    }

    public virtual void TurnEndActive() // 캐릭터가 턴이 끝날 때 호출되는 함수
    {
        _currento2 += 10.0f; // 현재 산소량 증가
        if (_currento2 > _maxo2) // 최대 산소량보다 높으면 최대산소로 제한
        {
            _currento2 = _maxo2;
        }
        if(_playerAct == _Act.Rescue) // 구조중이라면
        {
            _rescueTarget.RescueCount--; // 구조중인 대상의 남은 구조턴 감소
            if(_rescueTarget.RescueCount <= 0) // 구조턴 값이 0보다 작으면
            {
                _rescueTarget.gameObject.SetActive(false); // 구조 대상 숨기기
                _playerAct = _Act.Idle; // Idle 상태로 변경
            }
        }
    }

    public void Rescue() // 구조 버튼 누를 시 호출되는 함수
    {
        int RescueLayer = 1 << LayerMask.NameToLayer("Rescue"); // 생존자의 Layer
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _moveDir, 128, RescueLayer); // 레이캐스트 쏘기
        if (hit)
        {
            if (hit.transform.CompareTag("RescueTarget")) // 레이캐스트 충돌 대상이 구조대상이라면
            {
                _rescueTarget = hit.transform.GetComponent<RescueTarget>(); // 생존자 값 저장
                _playerAct = _Act.Rescue; // 구조 상태로 변경
            }
        }
        UI_Actives.SetActive(false); // UI 숨기기
    }

    public void OpenToolBtns() // 도구 버튼 누를 시 호출되는 함수
    {
        UI_ToolBtns.SetActive(true); // 도구 UI 보이기
    }

    public void UseTool(int toolnum) // 도구 UI의 버튼 누를 시 호출되는 함수
    {
        switch(toolnum)
        {
            case 1: // FireExtinguisher
                StopAllCoroutines();
                StartCoroutine(UseFireExtinguisher());
                break;
            case 2: // StickyBomb
                UseStickyBomb();
                break;
            case 3: // FireWall
                UseFireWall();
                break;
            case 4: // SmokeAbsorption
                break;
            case 5: // O2Can
                UseO2Can();
                break;
        }
        UI_Actives.SetActive(false);
        UI_ToolBtns.SetActive(false); // UI 숨기기
    }

    IEnumerator UseFireExtinguisher() // 소화기 사용 코드 
    {
        while(true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //좌표 값 변경으로 인해 수정해야 할 코드
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int nPos = new Vector3Int(((int)mousePos.x / 1000), (int)mousePos.y / 1000, 0); // 마우스의 타일맵 상 좌표
                if (mousePos.x - transform.position.x < 2000 &&
                    mousePos.x - transform.position.x > -2000 &&
                    mousePos.y - transform.position.y < 2000 &&
                    mousePos.y - transform.position.y > -2000) // 누른 위치가 캐릭터 기준 2칸 이내라면
                {
                    Debug.Log("In Range");
                    Debug.Log(nPos);
                    Vector3Int offset = Vector3Int.zero; // 소화기가 퍼져나갈 크기
                    if(mousePos.x > transform.position.x) // 마우스가 캐릭터보다 오른쪽에 있다면
                    {
                        offset.x = 1;
                    }
                    else if(mousePos.x < transform.position.x) // 마우스가 캐릭터보다 왼쪽에 있다면
                    {
                        offset.x = -1;
                    }
                    if(mousePos.y > transform.position.y) // 마우스가 캐릭터보다 위에 있다면
                    {
                        offset.y = 1;
                    }
                    else if(mousePos.y < transform.position.y) // 마우스가 캐릭터보다 아래 있다면
                    {
                        offset.y = -1;
                    }
                    if (GameMgr.Instance.Obstacle.GetTile(nPos).name == "Fire") // 탐색한 타일이 큰 불 타일이라면
                    {
                        GameMgr.Instance.Obstacle.SetTile(nPos, null); // 불 제거
                        //TileMgr의 Fire 리스트 속 ID를 검색받아 타일제거를 해야합니다 웅연쿤 // 고맙다 과거의 나
                        //이 아래부턴 소화기가 퍼져나간 곳의 불타일을 제거하는 코드
                        nPos.x += offset.x;
                        if (GameMgr.Instance.Obstacle.GetTile(nPos).name == "Fire")
                        {
                            GameMgr.Instance.Obstacle.SetTile(nPos, null);
                        }
                        nPos.x -= offset.x;
                        nPos.y += offset.y;
                        if (GameMgr.Instance.Obstacle.GetTile(nPos).name == "Fire")
                        {
                            GameMgr.Instance.Obstacle.SetTile(nPos, null);
                        }
                        nPos.x += offset.x;
                        if (GameMgr.Instance.Obstacle.GetTile(nPos).name == "Fire")
                        {
                            GameMgr.Instance.Obstacle.SetTile(nPos, null);
                        }
                    }
                    break;
                }
            }
            else // 누를때 까지 코루틴 대기
            {
                yield return null;
            }
        }
        yield return null;
    }

    private void UseFireWall() // 방화벽 설치
    {
        //좌표 단위가 변화되면서 수정해야할 코드
        Vector3Int nPos = new Vector3Int(((int)transform.position.x / 1000) + (int)_moveDir.x, ((int)transform.position.y + 500) / 1000, 0);
        Debug.Log(nPos);
        if (GameMgr.Instance.Obstacle.GetTile(nPos) == null) // 설치할 위치에 장애물이 없다면 방화벽 설치
        {
            GameMgr.Instance.Obstacle.SetTile(nPos, FireWallTile);
        }
    }

    private void UseStickyBomb() // 점착폭탄 설치
    {
        //좌표 단위가 변화되면서 수정해야할 코드
        int ObstacleLayer = 1 << LayerMask.NameToLayer("Obstacle"); // 장애물만 체크할 Layer
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 500), _moveDir, 128, ObstacleLayer); // 레이캐스트 발사=
        if (hit)
        {
            Debug.Log("Ray hit");
            Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Wall")) // 레이캐스트 충돌대상이 벽이라면
            {
                Vector3Int nPos = new Vector3Int(((int)transform.position.x / 1000) + (int)_moveDir.x, ((int)transform.position.y + 500) / 1000, 0);
                GameMgr.Instance.Obstacle.SetTile(nPos, null); // 벽 제거
            }
        }
    }

    private void UseO2Can() // 산소캔 사용
    {
        _currento2 += 45; // 현재 산소량 증가
        if (_currento2 > _maxo2) // 현재 산소량이 최대치 초과시 최대치로 제한
        {
            _currento2 = _maxo2;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) // 충돌체크
    {
        if(other.CompareTag("Fire")) // 충돌 대상이 큰 불이라면
        {
            _currentHp -= 25.0f; // 체력 감소
            HPGage.fillAmount = _currentHp / _maxHp; // 체력 UI 변경
            _currentMental-=2; // 멘탈 감소
            MTGage.fillAmount = _currentMental / _maxMental; // 멘탈 UI 변경
        }
        else if(other.CompareTag("Ember")) // 충돌 대상이 작은 불이라면
        {
            _currentHp -= 10.0f; // 체력 감소
            HPGage.fillAmount = _currentHp / _maxHp; // 체력 UI 감소
            _currentMental--; // 멘탈 감소
            MTGage.fillAmount = _currentMental / _maxMental; // 멘탈 UI 변경
        }
    }

    //private void SetFOV()
    //{
    //    GridLayout gridLayout = GameMgr.Instance.FogTile.GetComponent<GridLayout>();
    //    Vector3Int nPos = gridLayout.WorldToCell(transform.position + (Vector3.left * 2000));
    //    int count = 5 / 2;
    //    for(int i= -count; i<=count; i++)
    //    {
    //        int checkArea = 0;
    //        for(int j=0; j< 5 - Mathf.Abs(i); j++)
    //        {
    //            if(checkArea >= Mathf.Abs(i))
    //            {
    //                GameMgr.Instance.FogTile.SetTileFlags(nPos + new Vector3Int(checkArea, i, 0), TileFlags.None);
    //                GameMgr.Instance.FogTile.SetColor(nPos + new Vector3Int(checkArea, i, 0), new Color(1, 1, 1, 0.0f));
    //            }
    //            checkArea++;
    //        }
    //    }
    //}
}