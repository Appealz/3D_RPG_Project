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
        Debug.Log("�̵�");
        OnMoveEvent?.Invoke();
    }

    public void EndState()
    {
        Debug.Log("�̵� ����");
        OnMoveStop?.Invoke();
    }

    public void EnterState()
    {
        Debug.Log("�̵� �ʱ�ȭ");
        OnMoveStart?.Invoke();
    }
}

