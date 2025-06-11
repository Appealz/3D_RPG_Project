using System;
using UnityEngine;

public class NonTargetAreaSkill : NonTargetSkillBase
{
    public override event Action OnSkillActivated;
    public override event Action<StateType> OnStateChange;
    public override event Action OnActionCancel;
    public override event Action OnSkillFinish;

    public Vector3 targetPos;

    public bool isActive = true;
    public bool isAttacking = false;

    private Transform firePoint;
    private GameObject obj;


    public override void Activate()
    {
        ManualRotate();
        if (!isAttacking)
        {            
            OnSkillActivated?.Invoke();
                        
            isAttacking = true;
        }
    }

    public override void CreateEffect()
    {
        obj = ObjectPoolManager.Instance.pool[5].PopObj();
        if (obj == null)
        {
            Debug.Log("obj 참조안됨");
            return;
        }
        else
        {
            obj.transform.position = targetPos;
        }
        Skill_Event.InvokeNonTargetAreaSkillSpawn(new NonTargetAreaSkillInfo(fireOwner, targetPos, damage, ProjectileType.Rskill));
        Debug.Log("상태 변환 완료");
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
    }

    public override void Finish()
    {
        OnActionCancel?.Invoke();
        OnSkillFinish?.Invoke();
        isAttacking = false;
    }

    public void TargetPosSetting(Vector3 newPos)
    {
        targetPos = newPos;
    }

    public override void TargetPositionSetting(SkillTargetPositionEvent targetEvent)
    {
        targetPos = targetEvent.TargetPos;
    }


    public bool TargetDistanceCheck()
    {
        if (isAttacking)
        {
            return true;
        }

        return (targetPos - fireOwner.transform.position).sqrMagnitude <= realRange;
    }

    public override void CancelAble()
    {
        OnActionCancel?.Invoke();
    }



    public void ManualRotate()
    {
        Vector3 dir = targetPos - fireOwner.transform.position;
        dir.y = 0f; // y축 고정 (회전만 원할 때)

        if (dir == Vector3.zero) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        fireOwner.transform.rotation = Quaternion.Slerp(fireOwner.transform.rotation, targetRot, Time.deltaTime * 12f);
    }

    public void Release()
    {
        EventBus.UnSubscribe<SkillTargetPositionEvent>(TargetPositionSetting);
    }
}
