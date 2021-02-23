using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RescueTarget : MonoBehaviour
{
    public Image HPGage;

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
    private float _speed = 1000.0f;

    // Start is called before the first frame update
    protected virtual void Start()
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
    }

    public virtual void TurnEndActive()
    {
        if (GameMgr.Instance.GameTurn % 1 == 0)
        {
            StartCoroutine(Move());
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
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
        Vector3 arrivePos = GameMgr.Instance.BackTile.CellToWorld(nPos);
        while (_panicMoveCount > 0)
        {
            transform.Translate((Vector3)rPos * _speed * Time.deltaTime);
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
                arrivePos = GameMgr.Instance.BackTile.CellToWorld(nPos);

                _panicMoveCount--;
            }
        }
        _panicMoveCount = 2;
        Debug.Log("Panic Done");
    }
}
