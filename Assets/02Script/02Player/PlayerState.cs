using System;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,
    Move,
    Attack,
    Chase,
}

public class PlayerState : MonoBehaviour
{
    StateType curStateType;

    public event Action OnIdleEvent;
    public event Action OnMoveEvent;
    public event Action OnAttackEvent;
    public event Action OnChaseEvent;

    public void InitState()
    {
        curStateType = StateType.Idle;
        ChangeState(curStateType);
    }

    public void ChangeState(StateType newState)
    {
        if(curStateType != newState)
        {
            curStateType = newState;
            Debug.Log($"상태 변환 : {newState}");
        }        
    }

    public void UpdateState()
    {
        switch (curStateType)
        {
            case StateType.Idle:
                OnIdleEvent?.Invoke();
                break;
            case StateType.Move:
                OnMoveEvent?.Invoke();
                break;
            case StateType.Chase:
                OnChaseEvent?.Invoke();
                break;
            case StateType.Attack:
                OnAttackEvent?.Invoke();
                break;
        }
    }
}
