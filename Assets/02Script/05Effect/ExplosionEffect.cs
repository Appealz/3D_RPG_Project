using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine.UIElements;

public class ExplosionEffect : PoolLabel
{
    Rigidbody rb;
    Transform target;
    Vector3 moveDir;
    GameObject Owner;
    float damage;

    [SerializeField] private float range = 5f;
    [SerializeField] private float angle = 60f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask targetMask;


    private void OnEnable()
    {
        StartCoroutine(ReturnTime());
        Skill_Event.NonTargetAreaSkillSpawned += SettingInfo;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        Skill_Event.NonTargetAreaSkillSpawned -= SettingInfo;
    }

    public void SettingInfo(NonTargetAreaSkillInfo projInfo)
    {
        if (projInfo.myType == ProjectileType.Rskill)
        {
            Owner = projInfo.owner;
            damage = projInfo.damage;
        }
        UseSkill();
    }

    public void UseSkill()
    {
        // Step 2: 타겟 판정
        Collider[] candidates = Physics.OverlapSphere(transform.position, 3f, targetMask);

        List<Transform> validTargets = new List<Transform>();

        foreach (Collider col in candidates)
        {               
            validTargets.Add(col.transform);

            // 예시: 데미지 적용
            Damage_Event.TakeDamage(new DamageInfo(Owner, col.gameObject, damage, col.transform.position, DamageEffectType.Fire));
        }
        Debug.Log($"R에 맞은 대상 수: {validTargets.Count}");
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 origin = transform.position;        

        float step = 5f; // 각도 간격
        for (float i = -angle / 2f; i <= angle / 2f; i += step)
        {
            Quaternion rotation = Quaternion.AngleAxis(i, Vector3.up);
            Vector3 dir = transform.position;
            Gizmos.DrawRay(origin, dir * range);
        }

        // 시야의 전체 반지름 표시 (간단한 원)
        Gizmos.DrawWireSphere(origin, 3f);
    }

    IEnumerator ReturnTime()
    {        
        yield return new WaitForSeconds(0.5f);
        ReturnPool();
    }
}
