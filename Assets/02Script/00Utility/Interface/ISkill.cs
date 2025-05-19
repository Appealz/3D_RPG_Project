using System;
using UnityEngine;

public interface ISkill 
{
    SkillType myType { get; }
    string skillName { get; }
    float coolTime { get; }
    float damage { get; }
    
    event Action OnSkillActivated;
    void SetOwner(GameObject owner);
    void Activate();
    void TriggerEvent();
}
