using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

/// <summary>
/// 타겟, 발사주체, 데미지, 프로젝타일 타입을 정해주는 구조체
/// </summary>
public struct ProjectileInfo
{
    public Transform target;
    public GameObject owner;
    public float damage;
    public ProjectileType myType;

    /// <summary>
    /// 타겟(transform), 발사주체(Gameobject), 데미지(float), 프로젝타일 타입(ProjectileType)
    /// </summary>
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

public static class EventBus
{
    private static Dictionary<Type, Delegate> eventTable = new Dictionary<Type, Delegate>();

    public static void Subscribe<T>(Action<T> newMethod)
    {
        if (eventTable.TryGetValue(typeof(T), out var existMethod))
        {
            eventTable[typeof(T)] = Delegate.Combine(existMethod, newMethod);
        }
        else
        {
            eventTable[typeof(T)] = newMethod;
        }
    }

    public static void UnSubscribe<T>(Action<T> removeMethod)
    {
        if (eventTable.TryGetValue(typeof(T), out var existMethod))
        {
            var newDelegate = Delegate.Remove(existMethod, removeMethod);
            if (newDelegate == null)
            {
                eventTable.Remove(typeof(T));
            }
            else
            {
                eventTable[typeof(T)] = newDelegate;
            }
        }
    }

    public static void Publish<T>(T type)
    {
        if (eventTable.TryGetValue(typeof(T), out var method))
        {
            (method as Action<T>)?.Invoke(type);
        }
    }
}