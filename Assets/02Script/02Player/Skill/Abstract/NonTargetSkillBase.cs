using UnityEngine;

public abstract class NonTargetSkillBase : SkillBase
{
    public override void TakeDamage()
    {
        base.TakeDamage();
    }
    public abstract void TargetPositionSetting(SkillTargetPositionEvent targetEvent);
}
