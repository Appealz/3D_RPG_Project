using System;
using UnityEngine;

public struct SkillPreparedEvent
{
    public SkillType SkillType;

    public SkillPreparedEvent(SkillType skillType)
    {
        SkillType = skillType;
    }
}
public class SkillActivatedEvent
{
    public SkillType SkillType { get; private set; }

    public SkillActivatedEvent(SkillType skillType)
    {
        SkillType = skillType;
    }
}

public class SkillAvailablityEvent
{
    public SkillType SkillType;
    public Action<bool> Callback;  // 답변 콜백

    public SkillAvailablityEvent(SkillType skillType, Action<bool> callback)
    {
        SkillType = skillType;
        Callback = callback;
    }
}
