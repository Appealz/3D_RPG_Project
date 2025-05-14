using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IAttack
{
    public event Action OnStopMove;
    public event Action OnAttackAnims;

    private bool isAttack;
    private bool isAttacking;

    GameObject obj;

    Transform firePoint;

    private float attackRate = 1f;

    private void Awake()
    {
        firePoint = FindObjectTransform.FindChildTransform(transform, "FirePoint");
    }

    public void Attack(Transform targetTrans)
    {   
        if(isAttack && !isAttacking)
        {
            StartCoroutine(AttackCoroutine(targetTrans));
        }
    }

    public void SetEnable(bool newEnable)
    {
        isAttack = newEnable;
    }

    IEnumerator AttackCoroutine(Transform targetTrans)
    {
        isAttacking = true;
        OnStopMove?.Invoke();
        OnAttackAnims?.Invoke();
        obj = ObjectPoolManager.Instance.pool.PopObj();
        obj.transform.position = firePoint.position;
        if (obj.TryGetComponent<Projectile>(out Projectile proj))
        {
            proj.TargetSetting(targetTrans);
            proj.SetEnable(true);
        }
        //Debug.Log($"АјАн : {targetTrans.name}, {targetTrans}");
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
    }
}
