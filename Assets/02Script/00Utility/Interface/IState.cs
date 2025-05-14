using UnityEngine;

public interface IState
{
    void EnterState();
    void StateUpdate();
    void EndState();
}
