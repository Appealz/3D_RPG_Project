using System;
using UnityEngine;

public interface ISkill 
{    
    SkillType myType { get; }
    string skillName { get; }
    float coolTime { get; }
    float damage { get; }
    float mpCost { get; }

    event Action OnSkillActivated;
    void Init(SkillData newData);

    void SetOwner(GameObject owner);
    void Activate();
    void TriggerEvent();
}
