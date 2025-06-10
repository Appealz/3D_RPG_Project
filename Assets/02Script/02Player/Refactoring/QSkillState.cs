using System;
using UnityEditor.Experimental.GraphView;
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
        targetSkill.targetTrans = player.targetTrans;
                
        isDone = false;

        player.Agent.velocity = Vector3.zero;

        targetSkill.TargetSet(player.targetTrans);

        if (player.targetTrans == null || !player.targetTrans.gameObject.activeSelf)
        {
            playerFSM.ChangeState(StateType.Idle);
        }
        if (targetSkill.TargetDistanceCheck() && !targetSkill.isAttacking)
        {
            targetSkill.Activate();
        }
    }

    public override void StateUpdate()
    {
        targetSkill.Rotation();
    }
    public override void StateExit()
    {
        isDone = true;
        targetSkill.OnActionCancel -= Cancel;
    }

    public override void Cancel()
    {
        base.Cancel();
        isDone = true;
    }



}
