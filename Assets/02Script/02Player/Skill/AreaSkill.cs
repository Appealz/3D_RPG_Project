using System;
using UnityEngine;

public class AreaSkill : NonTargetSkillBase, IRelease
{
    public override event Action OnSkillActivated;
    public override event Action<StateType> OnStateChange;

    public Vector3 targetPos;

    bool isActive = true;

    private Transform firePoint;
    private GameObject obj;
    public override void Activate()
    {        
        if (isActive)
        {   
            EventBus.Publish(new PlayerMoveLockEvent(false));
            OnSkillActivated?.Invoke();
            //CreateEffect();

            isActive = false;
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
        Skill_Event.InvokeNonTargetAreaSkillSpawn(new NonTargetAreaSkillInfo(fireOwner, targetPos, 10f, ProjectileType.Rskill));
        Debug.Log("상태 변환 완료");
    }

    public override void Finish()
    {
        OnStateChange?.Invoke(ActionQueue.Instance.DequeueAction());
        isActive = true;
    }

    public void Release()
    {
        EventBus.UnSubscribe<SkillTargetPositionEvent>(TargetPositionSetting);
    }

    public override void TargetPositionSetting(SkillTargetPositionEvent targetEvent)
    {
        targetPos = targetEvent.TargetPos;
    }

    private void TargetDistanceCheck()
    {
        float distSqr = (fireOwner.transform.position - targetPos).sqrMagnitude;
        Debug.Log($"[TargetSkill] distance: {distSqr}");

        if (distSqr > 100f)
        {
            ActionQueue.Instance.EnqueueAction(myState);
            EventBus.Publish(new TargetPositionEvent(targetPos));
            OnStateChange?.Invoke(StateType.Chase);
        }
        else
        {
            isActive = true;
        }
    }
}