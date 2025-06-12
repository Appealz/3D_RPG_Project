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

public class ChaseContext : IStateContext
{
    public float range;
    public ChaseContext(float newRange)
    {
        range = newRange;
    }
}

public class ChaseState : StateBase
{
    private readonly PlayerMove movement;
    private readonly PlayerAnims anims;
    private float attackRange;
    public ChaseState(Player player, PlayerFSM playerFSM, PlayerAnims newAnims, PlayerMove newMovement) : base(player, playerFSM)
    {
        movement = newMovement;
        anims = newAnims;
        attackRange = player.PlayerStatus.attackRagne;
        EventBus.Subscribe<cancleState>(CancelState);        
    }

    public override void StateEnter()
    {
        isDone = false;
        movement.StartMove(5f);
        movement.isOnSkill = true;
    }


    public override void StateUpdate()
    {
        if(player.targetTrans != null)
        {
            movement.SettingTarget(player.targetTrans);
            movement.Chase();

            if(movement.IsInRange(attackRange))
            {
                isDone = true;
            }
        }
        else
        {
            movement.SettingPoisition(player.targetPos);
            movement.Move();

            if (movement.IsInRangePosition(attackRange))
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
        movement.isOnSkill = false;
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

    public override void InjectContext(IStateContext context)
    {
        base.InjectContext(context);
        if(context is ChaseContext chaseContext)
        {
            attackRange = chaseContext.range;
            Debug.Log($"사거리 변경 : {attackRange}");
        }
        else
        {
            attackRange = player.PlayerStatus.attackRagne;
            Debug.Log($"사거리 변경 : {attackRange}");
        }
    }
}