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
        Debug.Log($"�÷��̾� ��ų �Ŵ��� status ����, ���� mp{playerStatus.CurMp}");
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

        // Ÿ���� ��ų
        if (skill is TargetSkillBase targetable)
        {
            EventBus.Subscribe<SkillTargetSelectedEvent>(targetable.TargetSetting);
        }
        // ��Ÿ���� ��ų
        if (skill is NonTargetSkillBase nontargetable)
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
        if (!IsSkillUse(preparedSkill.SkillType))
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
                switch (preparedSkill.SkillType)
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
        //Debug.Log("��ų ����");
        if (skills.Count > 0)
        {
            // 1. �÷��̾� �ִϸ��̼� ���ε�
            // 2. ��ų ��Ÿ�� �� ����
            // 3. ��ų mp �Ҹ� ����
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
        // ��ų�� ��Ÿ�� Ȯ���� ��ų ��밡������ && ��ų�� ��븶���� ���縶���� ���Ͽ� �������
        return skillModel.CanUseSkill(useSkillType) && (skills[useSkillType].mpCost < playerStatus.CurMp);
    }



    // ���� ����
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



    // �����ִ� ��ų�� ��Ÿ�� Ȯ��
    public float GetRemainingCoolTime(SkillType useSkillType) => skillModel.GetRemainingCoolTime(useSkillType);
    // ��ų�� �ִ� ��Ÿ�� Ȯ��.
    public float GetMaxCoolTime(SkillType useSkillType) => skillModel.GetMaxCoolTime(useSkillType);
}