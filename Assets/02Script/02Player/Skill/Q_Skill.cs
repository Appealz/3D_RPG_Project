using System;
using UnityEngine;

public class Q_Skill : MonoBehaviour, ISkill
{
    public SkillType myType => SkillType.Q_Skill;
    public string skillName => "Q_Skill";
    public float coolTime => 5f;
    public float damage => 10f;

    private GameObject fireOwner;
    
    public event Action OnSkillActivated;

    public void Activate()
    {
        OnSkillActivated?.Invoke();
        Debug.Log($"{myType} 스킬 발동");
    }

    public void SetOwner(GameObject owner)
    {
        fireOwner = owner;
    }

    public void TriggerEvent()
    {
        
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
