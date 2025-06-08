using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class QSkillState : StateBase
{
    TargetSkill targetSkill;

    public QSkillState(Player player, PlayerFSM playerFSM, ISkill newSkill) : base(player, playerFSM)
    {
        targetSkill = newSkill as TargetSkill;
        if (targetSkill == null)
        {
            throw new ArgumentException("QSkillState는 TargetSkill 타입만 지원합니다.");
        }
    }

    public override void StateEnter()
    {        
        targetSkill.targetPos = player.targetTrans;

        targetSkill.Activate();

        isDone = false;
    }

    public override void StateUpdate()
    {
        targetSkill.Rotation();
    }
    public override void StateExit()
    {
        isDone = true;
    }

    public override void Cancel()
    {
        base.Cancel();
        StateExit();
    }



}
