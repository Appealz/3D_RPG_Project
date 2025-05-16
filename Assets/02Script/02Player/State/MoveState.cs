using System;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : MonoBehaviour, IState
{
    //NavMeshAgent agnet;

    public event Action<StateType> OnStateChaged;

    public void StateUpdate()
    {
        Debug.Log("�̵�");
        if(transform.position == Vector3.zero)
        {
            OnStateChaged?.Invoke(StateType.Idle);
        }
    }

    public void EndState()
    {
        Debug.Log("�̵� ����");
    }

    public void EnterState()
    {
        Debug.Log("�̵� �ʱ�ȭ");
    }
}

