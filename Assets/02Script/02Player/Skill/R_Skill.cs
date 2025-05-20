using System;
using UnityEngine;

public class R_Skill : SkillBase, ISkill
{
    public override event Action OnSkillActivated;
    public override void Activate()
    {
        OnSkillActivated?.Invoke();
    }

    public override void TriggerEvent()
    {
        
    }
}