using System;
using UnityEngine;

public class ShieldSkill : BuffSkillBase
{
    public override event Action OnSkillActivated;
    public override event Action<StateType> OnStateChange;
    public override event Action OnActionCancel;
    public override event Action OnSkillFinish;

    Transform shieldPoint;
    GameObject obj;

    public bool isOn = true;

    public override void Activate()
    {
        if (isOn)
        {
            Debug.Log("E스킬 발동");
            OnSkillActivated?.Invoke();
            shieldPoint = FindObjectTransform.FindChildTransform(fireOwner.transform, "ShieldPoint");
            isOn = false;            
        }
    }

    public override void CancelAble()
    {
        OnActionCancel?.Invoke();
    }

    public override void CreateEffect()
    {
        obj = ObjectPoolManager.Instance.pool[4].PopObj();
        if (obj == null)
        {
            Debug.Log("obj 참조안됨");
            return;
        }
        else
        {
            obj.transform.position = shieldPoint.position;
            Skill_Event.InvokeShieldSkillSpawn(new ShieldSkillInfo(fireOwner, 10f, ProjectileType.Eskill));
        }
        Debug.Log("상태 변환 완료");
    }

    public override void Finish()
    {
        //OnStateChange?.Invoke(StateType.Idle);
        OnSkillFinish?.Invoke();    
        isOn = true;
    }

    
}
