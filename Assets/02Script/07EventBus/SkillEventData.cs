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

public class SkillEventData
{ 


}
