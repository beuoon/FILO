using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class RescueTarget : MonoBehaviour
{
    public Image HPGage;
    public Tile RTTile; // Rescue Target Tile
    public Image SmileMark;

    private enum _state { Panic, Static }
    private _state _RescueTargetState;

    protected int _rescueCount = 0;
    public int RescueCount
    {
        get { return _rescueCount; }
        set { _rescueCount = value; }
    }

    protected float _MaxHP = 60.0f;
    protected float _currentHP = 0.0f;
    private int _panicMoveCount = 2;
    private float _speed = 100.0f;

    // Start is called before the first frame update
    private void Start()
    {
        _currentHP = _MaxHP;
        switch(_RescueTargetState)
        {
            case _state.Panic:
                _rescueCount = 1;
                break;
            case _state.Static:
                _rescueCount = 2;
                break;
        }
        GameMgr.Instance.RescueTilemap.SetTile(GameMgr.Instance.RescueTilemap.WorldToCell(transform.position), RTTile);
    }

    public void TurnEndActive()
    {
        if (GameMgr.Instance.GameTurn % 1 == 0)
        {
            Debug.Log("Start");
            StartCoroutine(Move());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fire"))
        {
            _currentHP -= 25.0f;
            HPGage.fillAmount = _currentHP / _MaxHP;
            if(_currentHP / _MaxHP < 0.5f && _RescueTargetState == _state.Panic)
            {
                _rescueCount++;
                _RescueTargetState = _state.Static;
            }
        }
        else if (other.CompareTag("Ember"))
        {
            _currentHP -= 10.0f;
            HPGage.fillAmount = _currentHP / _MaxHP;
            if (_currentHP / _MaxHP < 0.5f && _RescueTargetState == _state.Panic)
            {
                _rescueCount++;
                _RescueTargetState = _state.Static;
            }
        }
    }

    public void ActiveSmileMark()
    {
        //StartCoroutine()
    }

    //IEnumerator DisableSmileMarkCounter()
    //{
    //    //yield return new WaitWhile(())
    //}

    IEnumerator Move()
    {
        int randx = 0;
        int randy = 0;
        while (true)
        {
            randx = Random.Range(-1, 2);
            randy = Random.Range(-1, 2);
            if (randx == 0 && randy == 0)
            {
                continue;
            }
            else
            {
                break;
            }
        }
        Vector3Int rPos = new Vector3Int(randx, randy, 0);
        Vector3Int nPos = GameMgr.Instance.BackTile.WorldToCell(transform.position) + rPos;
        Vector3 arrivePos = GameMgr.Instance.BackTile.CellToWorld(nPos) - (GameMgr.Instance.BackTile.cellSize / 2);
        while (_panicMoveCount > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, arrivePos, _speed * Time.deltaTime);
            yield return null;
            if (Vector3.Distance(arrivePos, transform.position) < 10f)
            {
                while (true)
                {
                    randx = Random.Range(-1, 2);
                    randy = Random.Range(-1, 2);
                    if (randx == 0 && randy == 0)
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                transform.position = new Vector3(arrivePos.x, arrivePos.y);
                rPos = new Vector3Int(randx, randy, 0);
                nPos = GameMgr.Instance.BackTile.WorldToCell(transform.position) + rPos;
                arrivePos = GameMgr.Instance.BackTile.CellToWorld(nPos) - (GameMgr.Instance.BackTile.cellSize / 2);

                _panicMoveCount--;
                GameMgr.Instance.RescueTilemap.SetTile(GameMgr.Instance.RescueTilemap.WorldToCell(transform.position) - new Vector3Int(randx, randy, 0), null);
                GameMgr.Instance.RescueTilemap.SetTile(GameMgr.Instance.RescueTilemap.WorldToCell(transform.position), RTTile);
            }
        }
        _panicMoveCount = 2;
        Debug.Log("Panic Done");
    }
}
