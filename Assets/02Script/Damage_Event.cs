using System;
using UnityEngine;

public struct DamageInfo
{
    public GameObject attacker;
    public GameObject defender;
    public float damage;

    public DamageInfo(GameObject newAttacker, GameObject newDefender, float newDamage)
    {
        attacker = newAttacker;
        defender = newDefender;
        damage = newDamage;
    }
}

public static class Damage_Event
{
    // 데미지를 받는 오브젝트들에 참조.
    public static event Action<DamageInfo> OnDamageChange;

    public static void TakeDamage(DamageInfo info)
    {
        OnDamageChange?.Invoke(info);
    }
}

public enum ProjectileType
{
    Normal,
    Qskill,
    Wskill,
    Eskill,
    Rskill,
}

public struct ProjectileInfo
{
    public Transform target;
    public GameObject owner;
    public float damage;
    public ProjectileType myType;

    public ProjectileInfo(Transform newTarget, GameObject newOwner, float newDamage, ProjectileType newType)
    {
        target = newTarget;
        owner = newOwner;
        damage = newDamage;
        myType = newType;
    }
}

public static class Skill_Event
{
    // 생성될 스킬 이펙트, 프로젝타일에 참조
    public static event Action<ProjectileInfo> ProjectileSpawned;

    public static void InvokeProjectileSpawn(ProjectileInfo info)
    {
        ProjectileSpawned?.Invoke(info);
    }
}