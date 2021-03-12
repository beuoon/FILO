using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Charactor : MonoBehaviour
{
    [SerializeField]
    protected float _maxo2 = 0.0f; // 캐릭터 최대 산소량
    protected float _currento2 = 0.0f; // 캐릭터 현재 산소량
    [SerializeField]
    protected float _useo2 = 0.0f; // 1초에 사용되는 산소량
    [SerializeField]
    protected float _maxHp = 0.0f; // 최대 체력
    protected float _currentHp = 0.0f; // 현재 체력

    public float MaxO2 // O2 Property
    {
        get { return _maxo2; }
    }
    public float CurrentO2 // 현재 O2 Property
    {
        get { return _currento2; }
    }
    public float MaxHP
    {
        get { return _maxHp; }
    }
    public float CurrentHP
    {
        get { return _currentHp; }
    }

    protected virtual void Start()
    {
        _currento2 = _maxo2;
        _currentHp = _maxHp;
    }

    public abstract void SetCurrentHP(float value);
    public abstract void SetCurrentO2(float value);
}
