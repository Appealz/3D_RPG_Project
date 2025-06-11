using System;
using UnityEngine;

public class NonTargettingSkill : NonTargetSkillBase
{
    public override event Action OnSkillActivated;
    public override event Action<StateType> OnStateChange;
    public override event Action OnActionCancel;
    public override event Action OnSkillFinish;

    Transform firePoint;
    public Vector3 targetPos;
    public bool isAttacking = false;


    public override void Activate()
    {
        ManualRotate();
        if (isAttacking)
        {
            return;
        }

        isAttacking = true;

        firePoint = FindObjectTransform.FindChildTransform(fireOwner.transform, "FirePoint");
        
        OnSkillActivated?.Invoke();
    }

    public override void CreateEffect()
    {
        GameObject obj = ObjectPoolManager.Instance.pool[3].PopObj();
        if (obj == null)
        {
            Debug.Log("obj 참조안됨");
            return;
        }
        else
        {
            obj.transform.rotation = fireOwner.transform.rotation;
            obj.transform.position = firePoint.transform.position;
        }
        Skill_Event.InvokeNonTargetSkillSpawn(new NonTargetSkillInfo(fireOwner, damage, ProjectileType.Wskill));
        Debug.Log("상태 변환 완료");
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
    }

    public override void Finish()
    {
        isAttacking = false;
        OnActionCancel?.Invoke();
        OnSkillFinish?.Invoke();
    }

    public void TargetPosSetting(Vector3 newTargetPos)
    {
        targetPos = newTargetPos;
    }

    public override void TargetPositionSetting(SkillTargetPositionEvent targetEvent)
    {
        targetPos = targetEvent.TargetPos;
    }

    public void Release()
    {
        EventBus.UnSubscribe<SkillTargetPositionEvent>(TargetPositionSetting);
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
        fireOwner.transform.rotation = Quaternion.Slerp(fireOwner.transform.rotation,targetRot,Time.deltaTime * 12f);        
    }

}
