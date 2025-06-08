using System;
using UnityEngine;

public abstract class TargetSkillBase : SkillBase
{
    protected TargetSkillBase(SkillData newData) : base(newData)
    {
    }

    public abstract void TargetSetting(SkillTargetSelectedEvent targetEvent);
}
