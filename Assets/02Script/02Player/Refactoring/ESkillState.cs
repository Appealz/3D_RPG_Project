using System;
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
        
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate()
    {
        
    }
}
