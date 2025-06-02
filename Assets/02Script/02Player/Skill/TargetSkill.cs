using System;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Playables;

public class TargetSkill : TargetSkillBase, IRelease
{
    public override event Action OnSkillActivated;        
    public override event Action<StateType> OnStateChange;

    

    public Transform targetPos;
        
    private bool isAttacking = false;

    private GameObject obj;

    private Transform firePoint;

    private bool firstActivated = false;    
    
    public override void Activate()
    {
        // 처음 스킬에 입장했을 때 한번만 실행
        if(!firstActivated)
        {
            firstActivated = true;
            TargetDistanceCheck();
            return;
        }

        // 공격중이라면 해당 메소드 탈출
        if (isAttacking)
        {
            return;
        }

        isAttacking = true;
        firePoint = FindObjectTransform.FindChildTransform(fireOwner.transform, "FirePoint");
        EventBus.Publish(new PlayerMoveLockEvent(false));
        OnSkillActivated?.Invoke();


        EventBus.Publish(new RotateToTargetEvent(targetPos));
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
        Debug.Log(" 스킬 종료");        
        isAttacking = false;        
        firstActivated = false;

        EventBus.Publish(new PlayerMoveLockEvent(true));
        OnStateChange?.Invoke(ActionQueue.Instance.DequeueAction());
    }

    public override void TargetSetting(SkillTargetSelectedEvent targetEvent)
    {
        targetPos = targetEvent.Target;
    }


    private void TargetDistanceCheck()
    {
        float distSqr = (fireOwner.transform.position - targetPos.position).sqrMagnitude;
        Debug.Log($"[TargetSkill] distance: {distSqr}");

        if (distSqr > (range * range))
        {            
            ActionQueue.Instance.EnqueueAction(myState);
            EventBus.Publish(new TargetSelectEvent(targetPos));
            OnStateChange?.Invoke(StateType.Chase);
        }
    }

    public void Release()
    {
        EventBus.UnSubscribe<SkillTargetSelectedEvent>(TargetSetting);        
    }
}
