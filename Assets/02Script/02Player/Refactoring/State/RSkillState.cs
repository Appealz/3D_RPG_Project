using System;
using UnityEngine;

public class RSkillState : StateBase
{
    NonTargetAreaSkill areaSkill;    

    public RSkillState(Player player, PlayerFSM playerFSM, ISkill newSkill) : base(player, playerFSM)
    {
        areaSkill = newSkill as NonTargetAreaSkill;
        if (areaSkill == null)
        {
            throw new ArgumentException("WSkillState NonTargetSkill 타입만 지원합니다.");
        }
    }

    public override void StateEnter()
    {        
        isDone = false;
        areaSkill.OnActionCancel += Cancel;
        areaSkill.OnSkillFinish += Finish;
        areaSkill.TargetPosSetting(player.targetPos);        
        areaSkill.ManualRotate();
        
        player.Agent.velocity = Vector3.zero;        

        if (areaSkill.TargetDistanceCheck() && !areaSkill.isAttacking)
        {
            areaSkill.Activate();
        }
    }

    public override void StateUpdate()
    {
        areaSkill.ManualRotate();
        
        if (!areaSkill.TargetDistanceCheck())
        {
            //Debug.Log("거리 안됨! 상태 바꾸려고 함.");
            playerFSM.ChangeState(StateType.Chase, force: true, new ChaseContext(areaSkill.realRange));
            ActionQueue.Instance.EnqueueAction(areaSkill.myState);
            return;
        }

        if (areaSkill.isAttacking)
            return;

        areaSkill.Activate();
    }

    public override void StateExit()
    {
        isDone = true;
        areaSkill.isAttacking = false;
        areaSkill.OnActionCancel -= Cancel;
        areaSkill.OnSkillFinish -= Finish;
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
