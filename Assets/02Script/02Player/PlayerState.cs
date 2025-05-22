using System;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,
    Move,
    Attack,
    Chase,
    SkillReady,
    SkillQ,
    SkillW,
    SkillE,
    SkillR,
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
            //ActionQueue.Instance.EnqueueAction(newState);
            //curStateType = ActionQueue.Instance.DequeueAction();
            //Debug.Log($"상태 변환 : {newState}");
            //Debug.Log($"현재 상태 : {curStateType}");
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
            //case StateType.SkillReady:                
            //    break;
        }
    }
}
