using UnityEngine;

public class MoveState : StateBase
{
    public MoveState(Player player, PlayerFSM playerFSM) : base(player, playerFSM)
    {
    }

    public override void StateEnter()
    {
        isDone = true;
        player.Agent.speed = 4f;
    }
    public override void StateUpdate()
    {
        player.Agent.SetDestination(player.targetPos);
    }

    public override void StateExit()
    {

    }


}