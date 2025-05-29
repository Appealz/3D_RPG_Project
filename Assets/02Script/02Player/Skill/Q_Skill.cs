using System;
using UnityEngine;

public class Q_Skill : SkillBase
{
    public override event Action OnSkillActivated;        
    public override event Action<StateType> OnStateChange;

    //private bool isAttack = true;

    public Transform targetPos;

    private bool isActive = true;
    private GameObject obj;

    private PlayerController playerController;

    private void Awake()
    {
        
        playerController = FindAnyObjectByType<PlayerController>();
    }


    public override void Activate()
    {

        OnSkillActivated?.Invoke();
        obj = ObjectPoolManager.Instance.pool[2].PopObj();
        //obj.transform.position = playerController.transform.position;
        Skill_Event.InvokeProjectileSpawn(new ProjectileInfo(targetPos, fireOwner, 10f, ProjectileType.Qskill));
        Debug.Log($"{gameObject.name} 스킬 발동");            
        Debug.Log("상태 변환 완료");
        OnStateChange?.Invoke(ActionQueue.Instance.DequeueAction());
    }

    public override void CreateEffect()
    {

     
    }

    public override void Finish()
    {
        OnStateChange?.Invoke(ActionQueue.Instance.DequeueAction());
        isActive = true;     
    }

    public override void TargetSetting(TargetSelectEvent targetEvent)
    {
        targetPos = targetEvent.Target;
    }
}
