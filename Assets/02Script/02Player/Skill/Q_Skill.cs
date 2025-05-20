using System;
using UnityEngine;

public class Q_Skill : SkillBase
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
