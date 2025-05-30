using System;
using UnityEngine;

public class AreaSkill : NonTargetSkillBase
{
    public override event Action OnSkillActivated;
    public override event Action<StateType> OnStateChange;

    public override void Activate()
    {
        OnSkillActivated?.Invoke();
        OnStateChange?.Invoke(ActionQueue.Instance.DequeueAction());
    }

    public override void CreateEffect()
    {
        
    }

    public override void Finish()
    {
        
    }

    public override void TargetPositionSetting(SkillTargetPositionEvent targetEvent)
    {
        //throw new NotImplementedException();
    }
}