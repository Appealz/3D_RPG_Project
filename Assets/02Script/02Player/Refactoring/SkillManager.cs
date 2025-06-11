using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.VisualScripting;

public class SkillManager : MonoBehaviour
{
    //private List<ISkill> skillList = new List<ISkill>();

    // 스킬 타입과 해당 타입에 따른 스킬 바인딩용 Dictionary
    private Dictionary<SkillType, ISkill> skills = new Dictionary<SkillType, ISkill>();

    // 스킬 타입과 애니메이션 바인딩용 Dictionary
    private Dictionary<SkillType, Action> skillAnimMap = new Dictionary<SkillType, Action>();

    // 스킬 타입과 상태 바인딩용 Dictionary
    private Dictionary<SkillType, StateType> skillStates = new Dictionary<SkillType, StateType>();

    private PlayerStatus_Fixed playerStatus;

    SkillModel skillModel;
    //public event Action<StateType> OnChangeState;

    private SkillType? preparedSkillType = null;

    public TargetSkill q_Skill;
    public NonTargetSkill w_Skill;
    public BarrierSkill e_Skill;
    public AreaSkill r_Skill;

    private void Awake()
    {
        skillModel = new SkillModel();
        //EventBus.Subscribe<SkillPreparedEvent>(PreparedSkill);
        //EventBus.Subscribe<SkillActivatedEvent>(UseSkill);

        EventBus.Subscribe<SkillAvailablityEvent>(OnSkillUse);
    }

    private void OnDisable()
    {
        //EventBus.UnSubscribe<SkillPreparedEvent>(PreparedSkill);
        //EventBus.UnSubscribe<SkillActivatedEvent>(UseSkill);

        EventBus.UnSubscribe<SkillAvailablityEvent>(OnSkillUse);
    }

    public void InitStatus(PlayerStatus_Fixed status)
    {
        playerStatus = status;
        //Debug.Log($"플레이어 스킬 매니저 status 장착, 현재 mp{playerStatus.CurMp}");
    }

    public void InitSkillAnimMap(PlayerAnims newAnims)
    {
        skillAnimMap[SkillType.Q_Skill] = newAnims.QSkillAnims;
        skillAnimMap[SkillType.W_Skill] = newAnims.WSkillAnims;
        skillAnimMap[SkillType.E_Skill] = newAnims.ESkillAnims;
        skillAnimMap[SkillType.R_Skill] = newAnims.RSkillAnims;
    }

    public void AddSkill(ISkill skill)
    {
        skills[skill.myType] = skill;
        //skillStates[skill.myType] = skill.myState;
        skill.SetOwner(gameObject);

        // 타겟형 스킬
        //if (skill is TargetSkillBase targetable)
        //{
        //    EventBus.Subscribe<SkillTargetSelectedEvent>(targetable.TargetSetting);
        //}
        //// 논타겟형 스킬
        //if (skill is NonTargetSkillBase nontargetable)
        //{
        //    EventBus.Subscribe<SkillTargetPositionEvent>(nontargetable.TargetPositionSetting);
        //}

        //if (skillAnimMap.TryGetValue(skill.myType, out var animAction))
        //{
        //    skill.OnSkillActivated += animAction;
        //}
        skill.OnSkillActivated += () => skillModel.UseSkill(skill.myType, skill.coolTime);
        skill.OnSkillActivated += () => ConsumeMp(skill.myType);
    }

    public ISkill GetSkill(SkillType type)
    {
        if (skills.TryGetValue(type, out var skill))
            return skill;
        Debug.LogError($"SkillType {type} not found");
        return null;
    }

    public void UpdateSKillCoolTIme()
    {
        skillModel.CoolTimeUpdate(Time.deltaTime);
    }

    public void PrepareSkill(SkillType preparedSkill)
    {
        //Debug.Log($"스킬 매니저에서 {preparedSkill} 준비");
        if (!IsSkillUse(preparedSkill))
        {
            //Debug.Log($"IsSkillUse {preparedSkill} 준비 안됨");
            preparedSkillType = null;
            return;
        }
        else
        {
            if (preparedSkill == SkillType.E_Skill)
            {
                UseSkill();                
            }
            else
            {
                switch (preparedSkill)
                {
                    case SkillType.Q_Skill:                        
                        EventBus.Publish(new indicatorEvent(IndicatorType.Circle, transform.position, skills[preparedSkill].range));
                        break;
                    case SkillType.R_Skill:                        
                        EventBus.Publish(new indicatorEvent(IndicatorType.Circle, transform.position, skills[preparedSkill].range));
                        EventBus.Publish(new indicatorEvent(IndicatorType.Area, Vector3.zero, 1f));
                        break;
                    case SkillType.W_Skill:
                        EventBus.Publish(new indicatorEvent(IndicatorType.Fan, transform.position, skills[preparedSkill].range));
                        break;
                }
                EventBus.Publish(new CursorEventData(cursorType.Aim));
                preparedSkillType = preparedSkill;
            }
        }
    }



    public StateType UseSkill()
    {
        StateType newStateType = StateType.Idle;
        if (!preparedSkillType.HasValue || !IsSkillUse(preparedSkillType.Value))
        {
            preparedSkillType = null;
            return newStateType;
        }
        //Debug.Log("스킬 눌림");

        //if (skillStates.TryGetValue(preparedSkillType.Value, out newStateType))
        //{
        //    // 1. 플레이어 애니메이션 바인딩
        //    // 2. 스킬 쿨타임 모델 연결
        //    // 3. 스킬 mp 소모 연결                        
        //    //ActionQueue.Instance.EnqueueAction(skillStates[preparedSkillType.Value]);            

        //}
        if(skills.TryGetValue(preparedSkillType.Value, out ISkill value))
        {
            newStateType = value.myState;
            EventBus.Publish(new CursorEventData(cursorType.Idle));
            EventBus.Publish(new HideIndicatorEvent());
            preparedSkillType = null;
        }

        return newStateType;
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
        Debug.Log($"쿨타임 사용 가능 : {skillModel.CanUseSkill(useSkillType)} 스킬 마나 사용 가능 : {(skills[useSkillType].mpCost < playerStatus.CurMp)}");
        Debug.Log($"쿨타임 사용 가능 : {skillModel.CanUseSkill(useSkillType)} 스킬 마나 : {skills[useSkillType].mpCost} 플레이어 마나 : {playerStatus.CurMp}");
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
