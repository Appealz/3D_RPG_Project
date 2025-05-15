using UnityEngine;

public class IdleState : IState
{
    PlayerState playerState;
    MoveState moveState;
    public void EndState()
    {
        
    }

    public void EnterState()
    {
        
    }

    public void StateUpdate()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Idle");
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            //playerState.ChangeState(moveState);
        }
    }


}
