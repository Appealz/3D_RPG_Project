using System;
using UnityEngine;

public interface IState
{
    event Action<StateType> OnStateChaged;
    void EnterState();
    void StateUpdate();
    void EndState();
}
