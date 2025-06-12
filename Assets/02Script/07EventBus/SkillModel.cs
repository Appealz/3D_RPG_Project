using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public struct SkillCoolDownEvent
{
    public SkillType Type;
    public float CoolDownTime;

    public SkillCoolDownEvent(SkillType newSkillType, float coolDownTime)
    {
        Type = newSkillType;
        CoolDownTime = coolDownTime;
    }
}



public class SkillModel
{
    private Dictionary<SkillType, float> skillCoolDowns = new Dictionary<SkillType, float>();
    private Dictionary<SkillType, float> skillMaxCoolTimes = new Dictionary<SkillType, float>();

    public void UseSkill(SkillType skillType, float coolDown)
    {
        if(!CanUseSkill(skillType))
        {
            return;
        }

        skillCoolDowns[skillType] = coolDown;
        skillMaxCoolTimes[skillType] = coolDown;

        EventBus.Publish(new SkillCoolDownEvent(skillType, coolDown));
    }
    public bool CanUseSkill(SkillType type)
    {        
        return !skillCoolDowns.ContainsKey(type) || skillCoolDowns[type] <= 0f;
    }
    public void CoolTimeUpdate(float deltaTIme)
    {
        var skillKeys = new List<SkillType>(skillCoolDowns.Keys);
        foreach(var skillType in skillKeys)
        {
            if (skillCoolDowns[skillType] > 0f)
            {
                skillCoolDowns[(skillType)] -= deltaTIme;
                if(skillCoolDowns[(skillType)] < 0f) // 스킬 쿨타임이 0보다 작아지면
                {
                    skillCoolDowns[(skillType)] = 0f;
                }
            }
        }
    }

    public float GetRemainingCoolTime(SkillType skillType)
    {
        return skillCoolDowns.ContainsKey(skillType) ? skillCoolDowns[(skillType)] : 0f;
    }

    public float GetMaxCoolTime(SkillType skillType)
    {
        return skillMaxCoolTimes.ContainsKey(skillType) ? (skillMaxCoolTimes[skillType]) : 0f;
    }
}



