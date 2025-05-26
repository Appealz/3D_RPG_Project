using System;
using UnityEngine;

public class Q_Skill : SkillBase
{
    public override event Action OnSkillActivated;        
    public override event Action<StateType> OnStateChange;

    private bool isAttack = true;

    public override void Activate()
    {
        //if(isAttack)
        //{
        //    isAttack = false;
            OnSkillActivated?.Invoke();
            Skill_Event.InvokeProjectileSpawn(new ProjectileInfo(null, fireOwner, 10f, ProjectileType.Qskill));
            Debug.Log($"{gameObject.name} 스킬 발동");
            OnStateChange?.Invoke(ActionQueue.Instance.DequeueAction());
            Debug.Log("상태 변환 완료");
        //}
    }

    public override void TriggerEvent()
    {
        isAttack = true;
    }


}
