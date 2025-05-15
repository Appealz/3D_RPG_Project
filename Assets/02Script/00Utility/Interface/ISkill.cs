using UnityEngine;

public interface ISkill 
{
    string skillName { get; }
    float coolTime { get; }
    float damage { get; }
    void SetOwner(GameObject owner);
    void Activate();
    void TriggerEvent();
}
