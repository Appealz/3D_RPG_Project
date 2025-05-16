using System;
using UnityEngine;

public class ChaseState : MonoBehaviour, IState
{
    public event Action<StateType> OnStateChaged;

    public void EndState()
    {
        throw new NotImplementedException();
    }

    public void EnterState()
    {
        throw new NotImplementedException();
    }

    public void StateUpdate()
    {
        OnStateChaged?.Invoke(StateType.Idle);
    }


}
