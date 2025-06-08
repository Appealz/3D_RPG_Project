using System;
using UnityEngine;

public class WSkillState : StateBase
{
    NonTargetSkill nonTargetSkill;

    public WSkillState(Player player, PlayerFSM playerFSM, ISkill newSkill) : base(player, playerFSM)
    {
        nonTargetSkill = newSkill as NonTargetSkill;
        if (nonTargetSkill == null)
        {
            throw new ArgumentException("WSkillState NonTargetSkill 타입만 지원합니다.");
        }
    }

    public override void StateEnter()
    {
        
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate()
    {
        
    }

}
