using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IAttack
{
    public event Action OnStopMove;
    public event Action OnAttackAnims;
    public event Action OnStartMove;
    public event Action<StateType> OnChangeState;


    private bool isAttack;
    private bool isAttacking;
    [SerializeField]
    private float attackRange;

    GameObject obj;

    Transform firePoint;
    Transform target;

    [SerializeField]
    private float attackRate;



    private void Awake()
    {
        firePoint = FindObjectTransform.FindChildTransform(transform, "FirePoint");
        attackRate = 1f;
        attackRange = 25f;
        PCInputManager.OnMouseTargetClick += TargetSetting;
       
    }

    public void TargetSetting(Transform targetTrans)
    {
        target = targetTrans;
    }

    public void Attack()
    {
        RotateTowardsTarget(target);
        if (!isAttacking && target)
        {   
            isAttacking = true;            
            OnAttackAnims?.Invoke();
        }
    }

    public void AttackEvent()
    {        
        StartCoroutine(AttackCoroutine());
    }

    public void SetEnable(bool newEnable)
    {
        isAttack = newEnable;
    }

    IEnumerator AttackCoroutine()
    {   
        obj = ObjectPoolManager.Instance.pool[0].PopObj();
        obj.transform.position = firePoint.position;
        if (obj.TryGetComponent<Projectile>(out Projectile proj))
        {
            proj.TargetSetting(target);
            proj.SetEnable(true);
        }
        
        yield return new WaitForSeconds(1f/attackRate);
        isAttacking = false;
        if (target && (target.position - transform.position).sqrMagnitude >= attackRange)
        {
            OnChangeState?.Invoke(StateType.Chase);
        }

        if (target == null)
        {
            OnChangeState?.Invoke(StateType.Idle);
        }        
    }


    void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // Y축 고정 (수평 회전만 원할 때)

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float rotationSpeed = 360f; // 회전 속도 (조절 가능)

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

}
