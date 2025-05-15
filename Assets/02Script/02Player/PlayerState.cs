using System;
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
    //IState curState;
    StateType curStateType;
    public static Action OnIdleEvent;
    public static Action OnMoveEvent;
    public static Action OnAttackEvent;
    public static Action OnChaseEvent;

    private void Awake()
    {
        
    }

    public void InitState()
    {
        curStateType = StateType.Idle;
    }

    public void ChangeState(StateType newState)
    {
        curStateType = newState;
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

    //public void InitState(IState state)
    //{
    //    curState = state;
    //    state.EnterState();
    //}

    //public void ChangeState(IState state)
    //{
    //    state.EndState();
    //    curState = state;
    //    state.EnterState();
    //}

    //public void Handle_UpdateState()
    //{
    //    curState.StateUpdate();
    //}

    //public void Move()
    //{

    //}



}
