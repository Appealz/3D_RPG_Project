using System;
using Unity.VisualScripting;
using UnityEngine;

public class ESkillState : StateBase
{
    ShieldSkill barrierSkill;

    public ESkillState(Player player, PlayerFSM playerFSM, ISkill newSkill) : base(player, playerFSM)
    {
        barrierSkill = newSkill as ShieldSkill;
        if (barrierSkill == null)
        {
            throw new ArgumentException("WSkillState NonTargetSkill 타입만 지원합니다.");
        }
    }

    public override void StateEnter()
    {
        isDone = false;
        barrierSkill.OnActionCancel += Cancel;
        barrierSkill.OnSkillFinish += Finish;
        barrierSkill.Activate();
        //player.Agent.velocity = Vector3.zero;
    }

    public override void StateUpdate()
    {
        
    }

    public override void StateExit()
    {
        isDone = true;
        barrierSkill.isOn = true;
        barrierSkill.OnActionCancel -= Cancel;
        barrierSkill.OnSkillFinish -= Finish;
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
