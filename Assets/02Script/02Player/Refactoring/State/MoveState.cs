using UnityEngine;
using UnityEngine.AI;

public class MoveState : StateBase
{
    private readonly PlayerMove movement;
    public MoveState(Player player, PlayerFSM playerFSM, PlayerMove newMovement) : base(player, playerFSM)
    {
        movement = newMovement;        
    }

    public override void StateEnter()
    {
        movement.StartMove(player.PlayerStatus.moveSpeed);
        movement.isOnSkill = false;
        isDone = true;
    }
    public override void StateUpdate()
    {        
        movement.SettingPoisition(player.targetPos);
        movement.Move();
    }

    public override void StateExit()
    {
        movement.StopMove();
    }
}

