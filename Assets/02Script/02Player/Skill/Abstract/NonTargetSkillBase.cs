using UnityEngine;

public abstract class NonTargetSkillBase : SkillBase
{
    protected NonTargetSkillBase(SkillData newData) : base(newData)
    {
    }

    public abstract void TargetPositionSetting(SkillTargetPositionEvent targetEvent);
}
