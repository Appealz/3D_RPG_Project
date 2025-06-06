using UnityEngine;

public class AttackState : StateBase
{
    public AttackState(Player player, PlayerFSM playerFSM) : base(player, playerFSM)
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
