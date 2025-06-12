using UnityEngine;

public class IdleState : StateBase
{
    public IdleState(Player player, PlayerFSM playerFSM) : base(player, playerFSM)
    {
    }

    public override void StateEnter()
    {
        isDone = true;
        player.Agent.ResetPath();
        player.Agent.velocity = Vector3.zero;
    }

    public override void StateExit()
    {
        isDone = true;
    }

    public override void StateUpdate()
    {
     
    }

}
