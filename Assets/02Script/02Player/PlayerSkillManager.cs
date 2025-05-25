using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    private List<ISkill> skillList = new List<ISkill>();

    private Dictionary<SkillType, ISkill> skills = new Dictionary<SkillType, ISkill>();
    private Dictionary<SkillType, Action> skillAnimMap = new Dictionary<SkillType, Action>();

    PlayerAnims playerAnims;

    SkillModel skillModel;

    //public event Action<StateType> OnChangeState;

    private void Awake()
    {
        TryGetComponent<PlayerAnims>(out playerAnims);
        InitSkillAnimMap();
        skillModel = new SkillModel();
        PCInputManager.OnSkillAvailablity += IsSkillUsable;
    }

    private void OnDisable()
    {
        PCInputManager.OnSkillAvailablity -= IsSkillUsable;
    }

    private void InitSkillAnimMap()
    {
        skillAnimMap[SkillType.Q_Skill] = playerAnims.QSkillAnims;
        skillAnimMap[SkillType.W_Skill] = playerAnims.WSkillAnims;
        skillAnimMap[SkillType.E_Skill] = playerAnims.ESkillAnims;
        skillAnimMap[SkillType.R_Skill] = playerAnims.RSkillAnims;
    }

    public void AddSkill(KeyCode keyType, ISkill skill)
    {
        skills[skill.myType] = skill;
        Debug.Log($"{skill.myType} 등록");
        skill.SetOwner(gameObject);
        Debug.Log($"{gameObject.name} 오너 등록");

        if(skillAnimMap.TryGetValue(skill.myType, out var animAction))
        {
            skill.OnSkillActivated += animAction;
        }
    }

    public void UpdateSKillCoolTIme()
    {
        skillModel.CoolTimeUpdate(Time.deltaTime);
    }

    public void UseSkill(SkillType useSkillType)
    {
        Debug.Log("스킬 눌림");
        if(skills.Count > 0)
        {
            skills[useSkillType].Activate();
            skillModel.UseSkill(useSkillType, skills[useSkillType].coolTime);
            //OnChangeState?.Invoke(StateType.SkillReady);
        }
    }

    public bool IsSkillUsable(SkillType useSkillType) => skillModel.CanUseSkill(useSkillType);
    public float GetRemainingCoolTime(SkillType useSkillType) => skillModel.GetRemainingCoolTime(useSkillType);
    public float GetMaxCoolTime(SkillType useSkillType) => skillModel.GetMaxCoolTime(useSkillType);
}
