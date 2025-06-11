using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public abstract class SkillBase : ISkill
{
    protected SkillData skillData;

    protected GameObject fireOwner;
    public float realRange => skillData.range * skillData.range;

    public abstract event Action OnSkillActivated;
    public abstract event Action<StateType> OnStateChange;
    public abstract event Action OnActionCancel;
    public abstract event Action OnSkillFinish;

    public virtual SkillType myType => skillData.skillType;
    public virtual string skillName => skillData.skillName;
    public virtual float coolTime => skillData.coolTime;
    public virtual float damage => skillData.damage;
    public virtual float mpCost => skillData.mpCost;
    public StateType myState => skillData.stateType;

    public virtual float range => skillData.range;
    

    /// <summary>
    /// 오너 설정
    /// </summary>
    /// <param name="owner"></param>
    public virtual void SetOwner(GameObject owner)
    {
        fireOwner = owner;
    }

    /// <summary>
    /// SkillData(Scriptable Object)연결
    /// </summary>
    /// <param name="newData"></param>
    public virtual void SetupData(SkillData newData)
    {
        skillData = newData;
        //Debug.Log($"myType : {myType}");
        //Debug.Log($"skillName : {skillName}");
        //Debug.Log($"coolTime : {coolTime}");
        //Debug.Log($"damage : {damage}");
        //Debug.Log($"mpCost : {mpCost}");
    }

    public abstract void Activate();
    public abstract void CreateEffect();
    public abstract void Finish();
    public abstract void CancelAble();

    public virtual void TakeDamage() { }
}
