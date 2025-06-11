using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TargettingSkill : TargetSkillBase, IRelease
{
    public override event Action OnSkillActivated;
    public override event Action<StateType> OnStateChange;

    public override event Action OnActionCancel;
    public override event Action OnSkillFinish;

    public Transform targetTrans;

    public bool isAttacking = false;

    private GameObject obj;

    private Transform firePoint;    

    
    public override void Activate()
    {
        // 공격중이라면 해당 메소드 탈출
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
        obj = ObjectPoolManager.Instance.pool[2].PopObj();
        obj.transform.position = firePoint.transform.position;
        Skill_Event.InvokeProjectileSpawn(new ProjectileInfo(targetTrans, fireOwner, damage, ProjectileType.Qskill));
        Debug.Log("상태 변환 완료");
    }

    public override void CancelAble()
    {
        OnActionCancel?.Invoke();
    }

    public override void Finish()
    {
        Debug.Log(" 스킬 종료");
        isAttacking = false;
        OnSkillFinish?.Invoke();


    }


    public override void TargetSet(Transform target)
    {
        base.TargetSet(target);
        targetTrans = target;
    }

    public bool TargetDistanceCheck()
    {
        if (isAttacking)
        {
            return true;
        }
        if (targetTrans == null || targetTrans.gameObject.activeSelf == false)
            return false;
                

        return (targetTrans.position - fireOwner.transform.position).sqrMagnitude <= realRange;
    }

    public void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = target.position - fireOwner.transform.position;
        direction.y = 0f; // Y축 고정 (수평 회전만 원할 때)

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float rotationSpeed = 720f; // 회전 속도 (조절 가능)
        fireOwner.transform.rotation = Quaternion.RotateTowards(fireOwner.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }


    public void Release()
    {
        EventBus.UnSubscribe<SkillTargetSelectedEvent>(TargetSetting);
    }

    public override void TargetSetting(SkillTargetSelectedEvent targetEvent)
    {
        targetTrans = targetEvent.Target;
    }
}
