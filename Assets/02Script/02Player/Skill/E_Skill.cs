using System;
using UnityEngine;

public class E_Skill : SkillBase, ISkill
{
    public override event Action OnSkillActivated;
    public override event Action<StateType> OnStateChange;

    public override void Activate()
    {
        OnSkillActivated?.Invoke();
        OnStateChange?.Invoke(ActionQueue.Instance.DequeueAction());
    }

    public override void TriggerEvent()
    {
        
    }


}
