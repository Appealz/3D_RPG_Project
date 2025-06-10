using System;
using UnityEngine;

public class ShieldSkill : BuffSkillBase
{
    public override event Action OnSkillActivated;
    public override event Action<StateType> OnStateChange;
    public override event Action OnActionCancel;

    public override void Activate()
    {
        throw new NotImplementedException();
    }

    public override void CancelAble()
    {
        throw new NotImplementedException();
    }

    public override void CreateEffect()
    {
        throw new NotImplementedException();
    }

    public override void Finish()
    {
        throw new NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
