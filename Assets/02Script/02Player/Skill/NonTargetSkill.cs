using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NonTargetSkill : NonTargetSkillBase, IRelease
{

    public override event Action OnSkillActivated;
    public override event Action<StateType> OnStateChange;
    public override event Action OnActionCancel;

    public Vector3 targetPos;

    private bool isActive = true;    

    private GameObject obj;

    private Transform firePoint;



    public override void Activate()
    {        
        EventBus.Publish(new RotateToPosEvent(targetPos));

        if (isActive)
        {
            
            firePoint = FindObjectTransform.FindChildTransform(fireOwner.transform, "FirePoint");
            EventBus.Publish(new PlayerMoveLockEvent(false));
            OnSkillActivated?.Invoke();            

            isActive = false;
        }        
    }

    public override void CreateEffect()
    {
        obj = ObjectPoolManager.Instance.pool[3].PopObj();
        if(obj == null)
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

    public override void Finish()
    {
        OnStateChange?.Invoke(ActionQueue.Instance.DequeueAction());
        isActive = true;
        
        EventBus.Publish(new PlayerMoveLockEvent(true));
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
        throw new NotImplementedException();
    }

    public override void TakeDamage()
    {
        base.TakeDamage();
    }
}
