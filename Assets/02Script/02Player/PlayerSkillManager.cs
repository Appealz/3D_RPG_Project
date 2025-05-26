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

    private PlayerStatus playerStatus;

    SkillModel skillModel;
    public event Action<StateType> OnChangeState;

    private SkillType? preparedSkillType = null;

    private void Awake()
    {
        TryGetComponent<PlayerAnims>(out playerAnims);
        InitSkillAnimMap();
        skillModel = new SkillModel();        
        PCInputManager.OnSkillAvailablity += IsSkillUse;
        PCInputManager.OnSkillActive += HandleSkillUseRequested;
    }

    private void OnDisable()
    {
        PCInputManager.OnSkillAvailablity -= IsSkillUse;
    }

    public void InitStatus(PlayerStatus status)
    {
        playerStatus = status;
        Debug.Log($"플레이어 스킬 매니저 status 장착, 현재 mp{playerStatus.CurMp}");
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
        skill.SetOwner(gameObject);

        if(skillAnimMap.TryGetValue(skill.myType, out var animAction))
        {
            skill.OnSkillActivated += animAction;
        }
    }

    public void UpdateSKillCoolTIme()
    {
        skillModel.CoolTimeUpdate(Time.deltaTime);
    }

    public void PreparedSkill(SkillType useSkillType)
    {        
        if(!IsSkillUse(useSkillType))
        {
            return;
        }

        if(useSkillType == SkillType.E_Skill)
        {
            UseSkill(useSkillType);
        }
        else
        {
            preparedSkillType = useSkillType;
        }
    }

    private void HandleSkillUseRequested()
    {
        if(preparedSkillType.HasValue)
        {
            UseSkill(preparedSkillType.Value);
            preparedSkillType = null;
        }
    }

    public void UseSkill(SkillType useSkillType)
    {
        Debug.Log("스킬 눌림");
        if(skills.Count > 0)
        {
            //skills[useSkillType].Activate();
            OnChangeState?.Invoke(skills[useSkillType].myState);
            skillModel.UseSkill(useSkillType, skills[useSkillType].coolTime);
            playerStatus.CurMp -= skills[useSkillType].mpCost;
            //OnChangeState?.Invoke(StateType.SkillReady);
        }
    }

    public bool IsSkillUse(SkillType useSkillType)
    {
        return (IsSkillUsableCoolTime(useSkillType) && IsSkillUsableMp(useSkillType));        
    }

    public bool IsSkillUsableCoolTime(SkillType useSkillType)
    {        
        return skillModel.CanUseSkill(useSkillType);
    }
    public bool IsSkillUsableMp(SkillType useSkillType)
    {
        return skills[useSkillType].mpCost < playerStatus.CurMp;
    }

    public float GetRemainingCoolTime(SkillType useSkillType) => skillModel.GetRemainingCoolTime(useSkillType);
    public float GetMaxCoolTime(SkillType useSkillType) => skillModel.GetMaxCoolTime(useSkillType);
}
