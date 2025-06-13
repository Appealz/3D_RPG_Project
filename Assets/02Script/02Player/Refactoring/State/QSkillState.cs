using System;
using UnityEngine;

public class QSkillState : StateBase
{
    TargettingSkill targetSkill;

    public QSkillState(Player player, PlayerFSM playerFSM, ISkill newSkill) : base(player, playerFSM)
    {
        targetSkill = newSkill as TargettingSkill;
        if (targetSkill == null)
        {
            throw new ArgumentException("QSkillState는 TargetSkill 타입만 지원합니다.");
        }
    }

    public override void StateEnter()
    {
        targetSkill.OnActionCancel += Cancel;
        targetSkill.OnSkillFinish += Finish;
        targetSkill.targetTrans = player.targetTrans;
                
        isDone = false;

        player.Agent.velocity = Vector3.zero;

        targetSkill.TargetSet(player.targetTrans);

        if (player.targetTrans == null || !player.targetTrans.gameObject.activeSelf)
        {
            if (player.targetPos != Vector3.zero)
            {
                playerFSM.ChangeState(StateType.Move, force: true);
            }
            else
            {
                playerFSM.ChangeState(StateType.Idle);
            }
            return;
        }
        if (targetSkill.TargetDistanceCheck() && !targetSkill.isAttacking)
        {
            targetSkill.Activate();
        }


    }

    public override void StateUpdate()
    {
        targetSkill.RotateTowardsTarget(player.targetTrans);

        if(!targetSkill.TargetDistanceCheck())
        {
            //Debug.Log("거리 안됨! 상태 바꾸려고 함.");
            playerFSM.ChangeState(StateType.Chase, force: true, new ChaseContext(targetSkill.realRange));
            ActionQueue.Instance.EnqueueAction(targetSkill.myState);
            return;
        }
        if (player.targetTrans == null || !player.targetTrans.gameObject.activeSelf)
        {
            playerFSM.ChangeState(StateType.Idle, force: true);
        }        
        

        if (targetSkill.isAttacking)
            return;



        targetSkill.Activate();


    }
    public override void StateExit()
    {
        isDone = true;
        targetSkill.OnActionCancel -= Cancel;
        targetSkill.OnSkillFinish -= Finish;
    }

    public override void Cancel()
    {
        base.Cancel();
        Debug.Log($"{GetType().Name} Cancel 실행");
        isDone = true;
    }

    public void Finish()
    {
        isDone = true;
        playerFSM.ChangeState(StateType.Idle, force: true);
    }

}
