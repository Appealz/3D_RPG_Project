using System;
using UnityEngine;

public interface ISkill 
{    
    StateType myState { get; }
    SkillType myType { get; }
    string skillName { get; }
    float coolTime { get; }
    float damage { get; }
    float mpCost { get; }

    float range { get; }
    event Action OnSkillActivated;
    event Action<StateType> OnStateChange;
    event Action OnActionCancel;
    void SetupData(SkillData newData);
    void SetOwner(GameObject owner);
    void Activate();   
    void CreateEffect();
    void Finish();
    void CancelAble();
    void TakeDamage();
}
