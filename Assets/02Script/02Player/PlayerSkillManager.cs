using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    List<ISkill> skillList = new List<ISkill>();

    Dictionary<SkillType, ISkill> skills = new Dictionary<SkillType, ISkill>();

    PlayerAnims playerAnims;

    private void Awake()
    {
        TryGetComponent<PlayerAnims>(out playerAnims);
    }

    public void AddSkill(KeyCode keyType, ISkill skill)
    {
        skills[skill.myType] = skill;
        Debug.Log($"{skill.myType} ���");
        skill.SetOwner(gameObject);
        Debug.Log($"{gameObject} ���� ���");

        if(skill.myType == SkillType.Q_Skill)
        {
            skill.OnSkillActivated += playerAnims.QSkillAnims;
        }

        if (skill.myType == SkillType.W_Skill)
        {
            skill.OnSkillActivated += playerAnims.WSkillAnims;
        }

        if (skill.myType == SkillType.E_Skill)
        {
            skill.OnSkillActivated += playerAnims.ESkillAnims;
        }

        if (skill.myType == SkillType.R_Skill)
        {
            skill.OnSkillActivated += playerAnims.RSkillAnims;
        }

    }

    public void UseSkill(SkillType useSkillType)
    {
        Debug.Log("��ų ����");
        if(skills.Count > 0)
        {
            skills[useSkillType].Activate();
        }
    }
}
