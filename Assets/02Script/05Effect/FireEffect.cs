using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

public class FireEffect : PoolLabel
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
        Skill_Event.NonTargetSkillSpawned += SettingInfo;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        Skill_Event.NonTargetSkillSpawned -= SettingInfo;
    }

    public void SettingInfo(NonTargetSkillInfo projInfo)
    {
        if (projInfo.myType == ProjectileType.Wskill)
        {
            Owner = projInfo.owner;
            damage = projInfo.damage;
        }
        firePoint = FindObjectTransform.FindChildTransform(Owner.transform, "FirePoint");
        UseWSkill();
    }

    

    IEnumerator ReturnTime()
    {        
        yield return new WaitForSeconds(0.5f);
        ReturnPool();
    }


    public void UseWSkill()
    {        
        // Step 2: 타겟 판정
        Collider[] candidates = Physics.OverlapSphere(firePoint.position, range, targetMask);

        List<Transform> validTargets = new List<Transform>();

        foreach (Collider col in candidates)
        {
            Vector3 dirToTarget = (col.transform.position - firePoint.position).normalized;
            float angleToTarget = Vector3.Angle(firePoint.forward, dirToTarget);

            if (angleToTarget <= angle / 2f)
            {
                validTargets.Add(col.transform);

                // 예시: 데미지 적용
                Damage_Event.TakeDamage(new DamageInfo(Owner, col.gameObject, 10f));
            }
        }
        Debug.Log($"W에 맞은 대상 수: {validTargets.Count}");
    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;

        Gizmos.color = Color.red;

        Vector3 origin = firePoint.position;
        Vector3 forward = firePoint.forward;

        float step = 5f; // 각도 간격
        for (float i = -angle / 2f; i <= angle / 2f; i += step)
        {
            Quaternion rotation = Quaternion.AngleAxis(i, Vector3.up);
            Vector3 dir = rotation * forward;
            Gizmos.DrawRay(origin, dir * range);
        }

        // 시야의 전체 반지름 표시 (간단한 원)
        Gizmos.DrawWireSphere(origin, range);
    }
}
