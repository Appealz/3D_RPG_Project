using System;
using TMPro;
using UnityEngine;

public class TargetSkill : TargetSkillBase
{
    public override event Action OnSkillActivated;        
    public override event Action<StateType> OnStateChange;

    //private bool isAttack = true;

    public Transform targetPos;

    private bool isActive = false;
    private bool isAttacking = false;

    private GameObject obj;

    private Transform firePoint;

    public override void Activate()
    {
        if(!isAttacking)
        {
            TargetDistanceCheck();
        }        
        if (isActive)
        {
            isAttacking = true;
            firePoint = FindObjectTransform.FindChildTransform(fireOwner.transform, "FirePoint");

            OnSkillActivated?.Invoke();
            //CreateEffect();

            isActive = false;
        }
        //OnStateChange?.Invoke(ActionQueue.Instance.DequeueAction());
        //Finish();
    }

    public override void CreateEffect()
    {
        obj = ObjectPoolManager.Instance.pool[2].PopObj();
        obj.transform.position = firePoint.transform.position;
        Skill_Event.InvokeProjectileSpawn(new ProjectileInfo(targetPos, fireOwner, damage, ProjectileType.Qskill));
        Debug.Log("상태 변환 완료");
    }

    public override void Finish()
    {
        OnStateChange?.Invoke(ActionQueue.Instance.DequeueAction());
        //isActive = true;
        isAttacking = false;    
    }

    public override void TargetSetting(TargetSelectEvent targetEvent)
    {
        targetPos = targetEvent.Target;
    }

    private void TargetDistanceCheck()
    {
        float distSqr = (fireOwner.transform.position - targetPos.position).sqrMagnitude;
        Debug.Log($"[TargetSkill] distance: {distSqr}");

        if (distSqr > 100f)
        {
            //ActionQueue.Instance.EnqueueAction(myState);
            OnStateChange?.Invoke(StateType.Chase);
        }
        else
        {
            isActive = true;
        }
    }
}
