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
    IState curState;
    StateType curStateType;

    Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

    public static Action OnIdleEvent;
    public static Action OnMoveEvent;
    public static Action OnAttackEvent;
    public static Action OnChaseEvent;

    private void Awake()
    {
        
    }

    public void BindStateTypetoState(StateType stateType, IState state)
    {
        states[stateType] = state;
        state.OnStateChaged += ChangeState;
    }

    public void InitState()
    {
        curStateType = StateType.Idle;
        ChangeState(curStateType);
    }

    public void ChangeState(StateType newState)
    {
        curState.EndState();
        curStateType = newState;
        curState = states[curStateType];
        curState.EnterState();        
    }

    public void UpdateState()
    {
        curState?.StateUpdate();

        //switch (curStateType)
        //{
        //    case StateType.Idle:
        //        OnIdleEvent?.Invoke();
        //        break;
        //    case StateType.Move:
        //        OnMoveEvent?.Invoke();
        //        break;
        //    case StateType.Chase:
        //        OnChaseEvent?.Invoke();
        //        break;
        //    case StateType.Attack:
        //        OnAttackEvent?.Invoke();
        //        break;
        //}
    }
}
