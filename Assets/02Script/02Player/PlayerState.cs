using System;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle,
    Move,
    Attack,
    Chase,    
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
    public event Action OnQSkillEvent;
    public event Action OnWSkillEvent;
    public event Action OnESkillEvent;
    public event Action OnRSkillEvent;

    public void InitState()
    {
        curStateType = StateType.Idle;
        ChangeState(curStateType);
    }

    public void ChangeState(StateType newState)
    {
        if (curStateType != newState)
        {               
            curStateType = newState;
        }
        Debug.Log($"현재상태 : {curStateType}");
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
            case StateType.SkillQ:
                OnQSkillEvent?.Invoke();
                break;
            case StateType.SkillW:
                OnWSkillEvent?.Invoke();
                break;
            case StateType.SkillE:
                OnESkillEvent?.Invoke();
                break;
            case StateType.SkillR:
                OnRSkillEvent?.Invoke();
                break;

        }
    }
}
