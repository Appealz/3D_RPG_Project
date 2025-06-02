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

    public static event Action indicatorOff;

    SkillModel skillModel;
    public event Action<StateType> OnChangeState;

    private SkillType? preparedSkillType = null;

    private void Awake()
    {        
        skillModel = new SkillModel();
        EventBus.Subscribe<SkillPreparedEvent>(PreparedSkill);
        EventBus.Subscribe<SkillActivatedEvent>(UseSkill);

        EventBus.Subscribe<SkillAvailablityEvent>(OnSkillUse);
    }

    private void OnDisable()
    {
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
                
        // 타겟형 스킬
        if (skill is TargetSkillBase targetable)
        {
            EventBus.Subscribe<SkillTargetSelectedEvent>(targetable.TargetSetting);
        }        
        // 논타겟형 스킬
        if(skill is NonTargetSkillBase nontargetable)
        {
            EventBus.Subscribe<SkillTargetPositionEvent>(nontargetable.TargetPositionSetting);
        }

        if (skillAnimMap.TryGetValue(skill.myType, out var animAction))
        {
            skill.OnSkillActivated += animAction;
        }
        skill.OnSkillActivated += () => skillModel.UseSkill(skill.myType, skills[skill.myType].coolTime);
        skill.OnSkillActivated += () => ConsumeMp(skill.myType);
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
                preparedSkillType = null;
            }
            else
            {
                switch(preparedSkill.SkillType)
                {
                    case SkillType.Q_Skill:
                        EventBus.Publish(new indicatorEvent(IndicatorType.Circle, transform.position, skills[preparedSkill.SkillType].range));
                        break;
                    case SkillType.R_Skill:
                        EventBus.Publish(new indicatorEvent(IndicatorType.Circle, transform.position, skills[preparedSkill.SkillType].range));
                        EventBus.Publish(new indicatorEvent(IndicatorType.Area, Vector3.zero, 1f));
                        break;
                    case SkillType.W_Skill:
                        EventBus.Publish(new indicatorEvent(IndicatorType.Fan, transform.position, skills[preparedSkill.SkillType].range));
                        break;  
                }
                
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
        //Debug.Log("스킬 눌림");
        if(skills.Count > 0)
        {            
            // 1. 플레이어 애니메이션 바인딩
            // 2. 스킬 쿨타임 모델 연결
            // 3. 스킬 mp 소모 연결
            OnChangeState?.Invoke(skills[skillActivatedEvent.SkillType].myState);
            EventBus.Publish(new CursorEventData(cursorType.Idle));
            EventBus.Publish(new HideIndicatorEvent());
        }
    }

    public void ConsumeMp(SkillType skilltype)
    {
        playerStatus.CurMp -= skills[skilltype].mpCost;
    }

    private void OnSkillUse(SkillAvailablityEvent skillEvent)
    {
        bool canUse = IsSkillUse(skillEvent.SkillType);
        skillEvent.Callback?.Invoke(canUse);
    }

    public bool IsSkillUse(SkillType useSkillType)
    {
        // 스킬의 쿨타임 확인후 스킬 사용가능한지 && 스킬의 사용마나가 현재마나와 비교하여 충분한지
        return skillModel.CanUseSkill(useSkillType) && (skills[useSkillType].mpCost < playerStatus.CurMp);        
    }

    // 구독 해제
    public void ReleaseAllSkills()
    {
        foreach (var skill in skills.Values)
        {
            if (skill is IRelease releasable)
            {
                releasable.Release();      
            }            
            skill.OnSkillActivated -= () => skillModel.UseSkill(skill.myType, skills[skill.myType].coolTime);
            skill.OnSkillActivated -= () => ConsumeMp(skill.myType);
        }
    }



    // 남아있는 스킬의 쿨타임 확인
    public float GetRemainingCoolTime(SkillType useSkillType) => skillModel.GetRemainingCoolTime(useSkillType);
    // 스킬의 최대 쿨타임 확인.
    public float GetMaxCoolTime(SkillType useSkillType) => skillModel.GetMaxCoolTime(useSkillType);
}