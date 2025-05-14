using System;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    IState curState;
    IdleState idleState = new IdleState();

    private void Awake()
    {
        InitState(idleState);
    }

    public void InitState(IState state)
    {
        curState = state;
        state.EnterState();
    }

    public void ChangeState(IState state)
    {
        state.EndState();
        curState = state;
        state.EnterState();
    }

    public void Handle_UpdateState()
    {
        curState.StateUpdate();
        //OnUpdateState?.Invoke();
    }


}
