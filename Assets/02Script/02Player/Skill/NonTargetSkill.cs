using System;
using UnityEngine;

public class NonTargetSkill : NonTargetSkillBase
{
    public override event Action OnSkillActivated;
    public override event Action<StateType> OnStateChange;

    public override void Activate()
    {
        OnSkillActivated?.Invoke();
        OnStateChange?.Invoke(ActionQueue.Instance.DequeueAction());
        Debug.Log("상태 변환 완료");
    }

    public override void CreateEffect()
    {

    }

    public override void Finish()
    {

    }


}
