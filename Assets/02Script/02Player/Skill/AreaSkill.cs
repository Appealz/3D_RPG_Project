using System;
using UnityEngine;

public class AreaSkill : NonTargetSkillBase, IRelease
{
    public override event Action OnSkillActivated;
    public override event Action<StateType> OnStateChange;

    public Vector3 targetPos;

    bool isActive = true;
    bool isAttacking = false;

    private Transform firePoint;
    private GameObject obj;

    private bool firstActivated = false;

    public override void Activate()
    {
        EventBus.Publish(new RotateToPosEvent(targetPos));
        if (!firstActivated)
        {
            firstActivated = true;
            TargetDistanceCheck();
            return;
        }        

        if (isActive)
        {   
            EventBus.Publish(new PlayerMoveLockEvent(false));
            OnSkillActivated?.Invoke();            

            isActive = false;
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

    public override void Finish()
    {
        OnStateChange?.Invoke(ActionQueue.Instance.DequeueAction());
        isActive = true;
        firstActivated = false;
    }

    public void Release()
    {
        EventBus.UnSubscribe<SkillTargetPositionEvent>(TargetPositionSetting);
    }

    public override void TargetPositionSetting(SkillTargetPositionEvent targetEvent)
    {
        targetPos = targetEvent.TargetPos;
    }

    public void Cancel()
    {
        isActive = true;
        isAttacking = false;  
        firstActivated = false;
    }

    private void TargetDistanceCheck()
    {
        float distSqr = (fireOwner.transform.position - targetPos).sqrMagnitude;
        Debug.Log($"[TargetSkill] distance: {distSqr}");

        if (distSqr > realRange)
        {            
            ActionQueue.Instance.EnqueueAction(myState);
            EventBus.Publish(new SkillTargetPositionEvent(targetPos));            
            OnStateChange?.Invoke(StateType.Chase);
        }
    }
}