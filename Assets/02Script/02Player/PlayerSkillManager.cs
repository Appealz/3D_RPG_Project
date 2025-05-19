using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    List<ISkill> skillList = new List<ISkill>();

    Dictionary<SkillType, ISkill> skills = new Dictionary<SkillType, ISkill>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddSkill(KeyCode keyType, ISkill skill)
    {
        skills[skill.myType] = skill;
        Debug.Log($"{skill.myType} 등록");
        // skill.OnSkillActivated += 
    }

    public void UseSkill(SkillType useSkillType)
    {
        Debug.Log("스킬 눌림");
        if(skills.Count > 0)
        {
            skills[useSkillType].Activate();
        }
    }
}
