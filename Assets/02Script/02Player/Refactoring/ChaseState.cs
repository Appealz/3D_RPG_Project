using UnityEngine;

public class ChaseState : StateBase
{
    public ChaseState(Player player, PlayerFSM playerFSM) : base(player, playerFSM)
    {
    }

    public override void StateEnter()
    {
        isDone = false;
    }

    public override void StateExit()
    {

    }

    public override void StateUpdate()
    {

    }
}