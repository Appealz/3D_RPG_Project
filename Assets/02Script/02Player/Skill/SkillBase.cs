using System;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour,ISkill
{
    protected SkillData skillData;
    protected GameObject fireOwner;

    public abstract event Action OnSkillActivated;

    public virtual SkillType myType => skillData.skillType;
    public virtual string skillName => skillData.skillName;
    public virtual float coolTime => skillData.coolTime;
    public virtual float damage => skillData.damage;
    public virtual float mpCost => skillData.mpCost;
    
    public virtual void SetOwner(GameObject owner)
    {
        fireOwner = owner;
    }

    public virtual void Init(SkillData newData)
    {
        skillData = newData;
        Debug.Log($"myType : {myType}");
        Debug.Log($"skillName : {skillName}");
        Debug.Log($"coolTime : {coolTime}");
        Debug.Log($"damage : {damage}");
        Debug.Log($"mpCost : {mpCost}");
    }

    public abstract void Activate();
    public abstract void TriggerEvent();

}
