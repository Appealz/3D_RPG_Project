using UnityEngine;
using UnityEngine.AI;

public struct cancleState
{
    public bool isDone;

    public cancleState(bool newDone)
    {
        isDone = newDone;
    }

}

public class ChaseState : StateBase
{
    private readonly PlayerMove movement;
    private readonly PlayerAnims anims;
    public ChaseState(Player player, PlayerFSM playerFSM, PlayerAnims newAnims, PlayerMove newMovement) : base(player, playerFSM)
    {
        movement = newMovement;
        anims = newAnims;
        EventBus.Subscribe<cancleState>(CancelState);        
    }

    public override void StateEnter()
    {
        isDone = false;
        movement.StartMove(5f);        
    }


    public override void StateUpdate()
    {
        if(player.targetTrans != null)
        {
            movement.SettingTarget(player.targetTrans);
            movement.Chase();

            if(movement.IsInRange(player.PlayerStatus.attackRagne))
            {
                isDone = true;
            }
        }
    }

    public override void StateExit()
    {        
        //agent.ResetPath();
        //agent.velocity = Vector3.zero;
        movement.StopMove();
    }

    public override void Cancel()
    {
        base.Cancel();
        isDone = true;
        StateExit();
    }

    public void CancelState(cancleState cancleState)
    {
        Cancel();
    }
}