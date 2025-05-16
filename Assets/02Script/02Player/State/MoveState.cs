using System;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : MonoBehaviour, IState
{
    public event Action<StateType> OnStateChaged;
    public event Action OnMoveEvent;
    public event Action OnMoveStart;
    public event Action OnMoveStop;


    public void StateUpdate()
    {
        Debug.Log("이동");
        OnMoveEvent?.Invoke();
    }

    public void EndState()
    {
        Debug.Log("이동 정지");
        OnMoveStop?.Invoke();
    }

    public void EnterState()
    {
        Debug.Log("이동 초기화");
        OnMoveStart?.Invoke();
    }
}

