using System;
using UnityEngine;

public static class SkillFactory
{
    public static ISkill CreateSkill(SkillData newSkillData)
    {
        ISkill skill = newSkillData.skillType switch
        {
            SkillType.Q_Skill => new TargettingSkill(),
            SkillType.W_Skill => new NonTargettingSkill(),
            SkillType.E_Skill => new ShieldSkill(),
            SkillType.R_Skill => new NonTargetAreaSkill(),
            _ => throw new ArgumentOutOfRangeException("알 수 없는 스킬 타입입니다.")
        };

        skill.SetupData(newSkillData);
        return skill;
    }
}
