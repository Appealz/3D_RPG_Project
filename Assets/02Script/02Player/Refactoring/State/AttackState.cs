using UnityEngine;

public class AttackState : StateBase
{
    private readonly PlayerAttackHandle attackHandle;
    public AttackState(Player player, PlayerFSM playerFSM, PlayerAttackHandle newAttackHandle) : base(player, playerFSM)
    {
        attackHandle = newAttackHandle;
        //EventBus.Subscribe<cancleState>(CancelState);
        
    } 
    public override void StateEnter()
    {
        attackHandle.OnActionCancel += Cancel;
        isDone = false;
        player.Agent.velocity = Vector3.zero;
        
        attackHandle.TargetSetting(player.targetTrans);

        if (player.targetTrans == null || !player.targetTrans.gameObject.activeSelf)
        {
            playerFSM.ChangeState(StateType.Idle);
        }
        if (attackHandle.CheckTargetDistance() && !attackHandle.IsAttacking)
        {
            attackHandle.Attack();
        }
    }

    public override void StateUpdate()
    {
        // 타겟 null check
        if (player.targetTrans == null || !player.targetTrans.gameObject.activeSelf)
        {            
            playerFSM.ChangeState(StateType.Idle, force: true);
        }

        attackHandle.RotateTowardsTarget(player.targetTrans);

        // 거리 체크
        if (!attackHandle.CheckTargetDistance())
        {
            Debug.Log("거리 안됨! 상태 바꾸려고 함.");
            playerFSM.ChangeState(StateType.Chase, force: true);
            ActionQueue.Instance.EnqueueAction(StateType.Attack);
            return;
        }

        if (attackHandle.IsAttacking)
            return;


        attackHandle.Attack();
        //isDone = true;
    }

    public override void StateExit()
    {
        isDone = true;
        attackHandle.OnActionCancel -= Cancel;
    }

    public override void Cancel()
    {
        base.Cancel();
        isDone = true;        
    }

    public void CancelState(cancleState cancleState)
    {
        Cancel();
    }
}
