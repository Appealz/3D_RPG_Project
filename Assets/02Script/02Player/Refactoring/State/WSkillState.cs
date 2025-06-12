using System;
using UnityEngine;

public class WSkillState : StateBase
{
    NonTargettingSkill nonTargetSkill;

    public WSkillState(Player player, PlayerFSM playerFSM, ISkill newSkill) : base(player, playerFSM)
    {
        nonTargetSkill = newSkill as NonTargettingSkill;
        if (nonTargetSkill == null)
        {
            throw new ArgumentException("WSkillState NonTargetSkill 타입만 지원합니다.");
        }
    }

    public override void StateEnter()
    {
        isDone = false;
        nonTargetSkill.OnActionCancel += Cancel;
        nonTargetSkill.OnSkillFinish += Finish;
        nonTargetSkill.TargetPosSetting(player.targetPos);
        nonTargetSkill.Activate();
        nonTargetSkill.ManualRotate();
    }  

    public override void StateUpdate()
    {        
        nonTargetSkill.ManualRotate();
    }

    public override void StateExit()
    {
        isDone = true;
        nonTargetSkill.isAttacking = false;
        nonTargetSkill.OnActionCancel -= Cancel;
        nonTargetSkill.OnSkillFinish -= Finish;
    }

    public override void Cancel()
    {
        base.Cancel();
        isDone = true;
    }

    public override void Finish()
    {
        base.Finish();
        
    }

}
