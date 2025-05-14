using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IAttack
{
    public event Action OnStopMove;
    public event Action OnAttackAnims;
    public event Action OnStartMove;

    private bool isAttack;
    private bool isAttacking;

    GameObject obj;

    Transform firePoint;
    Transform target;

    [SerializeField]
    private float attackRate;

    private void Awake()
    {
        firePoint = FindObjectTransform.FindChildTransform(transform, "FirePoint");
        attackRate = 1f;
    }

    public void TargetSetting(Transform targetTrans)
    {
        target = targetTrans;
    }

    public void AttackEvent()
    {
        if (!isAttacking && isAttack)
        {
            isAttacking = true;
            OnStopMove?.Invoke();            
            OnAttackAnims?.Invoke();
        }
    }

    public void Attack()
    {
        StartCoroutine(AttackCoroutine());                           
    }

    public void SetEnable(bool newEnable)
    {
        isAttack = newEnable;
    }

    IEnumerator AttackCoroutine()
    {
        //OnAttackAnims?.Invoke();
        obj = ObjectPoolManager.Instance.pool.PopObj();
        obj.transform.position = firePoint.position;
        if (obj.TryGetComponent<Projectile>(out Projectile proj))
        {
            proj.TargetSetting(target);
            proj.SetEnable(true);
        }
        //Debug.Log($"АјАн : {targetTrans.name}, {targetTrans}");
        yield return new WaitForSeconds(attackRate);
        isAttacking = false;
        OnStartMove?.Invoke();
    }
}
