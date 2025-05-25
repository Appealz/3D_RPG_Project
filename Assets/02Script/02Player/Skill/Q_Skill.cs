using System;
using UnityEngine;

public class Q_Skill : SkillBase
{
    public override event Action OnSkillActivated;
    public override void Activate()
    {
        OnSkillActivated?.Invoke();
        Skill_Event.InvokeProjectileSpawn(new ProjectileInfo(null, fireOwner, 10f, ProjectileType.Qskill));
        Debug.Log($"{gameObject.name} 스킬 발동");
    }

    public override void TriggerEvent()
    {
        
    }


}
