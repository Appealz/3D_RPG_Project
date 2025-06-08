using System;
using UnityEngine;

public class RSkillState : StateBase
{
    AreaSkill areaSkill;

    public RSkillState(Player player, PlayerFSM playerFSM, ISkill newSkill) : base(player, playerFSM)
    {
        areaSkill = newSkill as AreaSkill;
        if (areaSkill == null)
        {
            throw new ArgumentException("WSkillState NonTargetSkill 타입만 지원합니다.");
        }
    }

    public override void StateEnter()
    {
        areaSkill.targetPos = player.targetPos;
        areaSkill.Activate();
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate()
    {
        
    }
}
