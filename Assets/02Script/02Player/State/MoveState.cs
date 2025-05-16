using System;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : MonoBehaviour, IState
{
    //NavMeshAgent agnet;

    public event Action<StateType> OnStateChaged;

    public void StateUpdate()
    {
        Debug.Log("이동");
        if(transform.position == Vector3.zero)
        {
            OnStateChaged?.Invoke(StateType.Idle);
        }
    }

    public void EndState()
    {
        Debug.Log("이동 정지");
    }

    public void EnterState()
    {
        Debug.Log("이동 초기화");
    }
}

