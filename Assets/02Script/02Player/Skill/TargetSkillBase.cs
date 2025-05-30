using System;
using UnityEngine;

public abstract class TargetSkillBase : SkillBase
{    
    public abstract void TargetSetting(SkillTargetSelectedEvent targetEvent);
}
