using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSkillManager : MonoBehaviour
{
    //private List<ISkill> skillList = new List<ISkill>();

    private Dictionary<SkillType, ISkill> skills = new Dictionary<SkillType, ISkill>();
    private Dictionary<SkillType, Action> skillAnimMap = new Dictionary<SkillType, Action>();
        

    private PlayerStatus playerStatus;

    SkillModel skillModel;
    public event Action<StateType> OnChangeState;

    private SkillType? preparedSkillType = null;

    private void Awake()
    {        
        skillModel = new SkillModel();
        EventBus.Subscribe<SkillPreparedEvent>(PreparedSkill);
        EventBus.Subscribe<SkillActivatedEvent>(UseSkill);

        EventBus.Subscribe<SkillAvailablityEvent>(OnSkillUse);

        //PCInputManager.OnSkillAvailablity += IsSkillUse;
        //PCInputManager.OnSkillActive += HandleSkillUseRequested;
    }

    private void OnDisable()
    {
        //PCInputManager.OnSkillAvailablity -= IsSkillUse;
        //PCInputManager.OnSkillActive -= HandleSkillUseRequested;

        EventBus.UnSubscribe<SkillPreparedEvent>(PreparedSkill);
        EventBus.UnSubscribe<SkillActivatedEvent>(UseSkill);

        EventBus.UnSubscribe<SkillAvailablityEvent>(OnSkillUse);
    }

    public void InitStatus(PlayerStatus status)
    {
        playerStatus = status;
        Debug.Log($"플레이어 스킬 매니저 status 장착, 현재 mp{playerStatus.CurMp}");
    }

    public void InitSkillAnimMap(PlayerAnims newAnims)
    {        
        skillAnimMap[SkillType.Q_Skill] = newAnims.QSkillAnims;
        skillAnimMap[SkillType.W_Skill] = newAnims.WSkillAnims;
        skillAnimMap[SkillType.E_Skill] = newAnims.ESkillAnims;
        skillAnimMap[SkillType.R_Skill] = newAnims.RSkillAnims;
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

    public void PreparedSkill(SkillPreparedEvent preparedSkill)
    {
        if(!IsSkillUse(preparedSkill.SkillType))
        {
            preparedSkillType = null;
            return;
        }
        else
        {
            if (preparedSkill.SkillType == SkillType.E_Skill)
            {
                UseSkill(new SkillActivatedEvent(preparedSkill.SkillType));
                EventBus.Publish(new CursorEventData(cursorType.Idle)); 
                preparedSkillType = null;
            }
            else
            {
                EventBus.Publish(new CursorEventData(cursorType.Aim));
                preparedSkillType = preparedSkill.SkillType;
            }
        }
    }

    public void UseSkill(SkillActivatedEvent skillActivatedEvent)
    {
        if (!IsSkillUse(skillActivatedEvent.SkillType))
        {
            preparedSkillType = null;
            return;
        }
        Debug.Log("스킬 눌림");
        if(skills.Count > 0)
        {            
            OnChangeState?.Invoke(skills[skillActivatedEvent.SkillType].myState);
            // 쿨타임모델 호출
            skillModel.UseSkill(skillActivatedEvent.SkillType, skills[skillActivatedEvent.SkillType].coolTime);
            // mp소모
            playerStatus.CurMp -= skills[skillActivatedEvent.SkillType].mpCost;         
        }
    }


    private void OnSkillUse(SkillAvailablityEvent skillEvent)
    {
        bool canUse = (IsSkillUsableCoolTime(skillEvent.SkillType) && IsSkillUsableMp(skillEvent.SkillType));
        skillEvent.Callback?.Invoke(canUse);
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
