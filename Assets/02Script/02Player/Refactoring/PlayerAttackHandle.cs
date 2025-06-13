using System;
using System.Collections;
using UnityEngine;

public class PlayerAttackHandle : MonoBehaviour
{

    public event Action OnActionCancel;
        
    private bool isAttacking;
    public bool IsAttacking => isAttacking;
        
    private float attackRange;
    GameObject obj;
    Transform firePoint;    
    Transform target;        
    private float attackRate;
    private float attackDamage;

    private void Awake()
    {
        firePoint = FindObjectTransform.FindChildTransform(transform, "FirePoint");
        if (firePoint == null)
        {
            Debug.Log($"{gameObject.name} : PlayerAttack.cs - Awake() - firePoint is not ref");
        }
        attackRate = 1f;
        attackRange = 25f;
        attackDamage = 10f;
     
        EventBus.Subscribe<RotateToTargetEvent>(RotateEvent);
    }

    private void OnDisable()
    {        
        EventBus.UnSubscribe<RotateToTargetEvent>(RotateEvent);
    }

    PlayerAnims anims;
    public void InitAttack(PlayerStatus_Fixed newStatus, PlayerAnims newAnims)
    {
        attackRange = newStatus.attackRagne;
        attackDamage = newStatus.attackDamage;
        attackRate = newStatus.attackRate;
        anims = newAnims;
    }

    public void TargetSetting(Transform newTarget)
    {
        target = newTarget;
    }

    public bool CheckTargetDistance()
    {
        if(isAttacking)
        {
            return true;
        }
        if (target == null || target.gameObject.activeSelf == false)
            return false;

        return (target.position - transform.position).sqrMagnitude <= attackRange;
    }

    public void Attack()
    {
        if (target == null || target.gameObject.activeSelf == false)
        {            
            return;
        }
        RotateTowardsTarget(target);
        

        if (!isAttacking && target)
        {
            isAttacking = true;
            Debug.Log($"{isAttacking} 공격 시작");
            anims.AttackAnims();
        }
    }

    public void AttackEvent()
    {
        if (target == null) return;
        StartCoroutine(AttackCoroutine());
               
    }

    private void AttackCancel()
    {
        OnActionCancel?.Invoke();
    }

    IEnumerator AttackCoroutine()
    {
        if (target == null || !target.gameObject.activeSelf)
        {
            isAttacking = false;
            yield break;
        }

        obj = ObjectPoolManager.Instance.pool[0].PopObj();
        obj.transform.position = firePoint.position;
        Skill_Event.InvokeProjectileSpawn(new ProjectileInfo(target, gameObject, attackDamage, ProjectileType.Normal));
        yield return new WaitForSeconds(0.1f / attackRate);
        OnActionCancel?.Invoke();
        yield return new WaitForSeconds(1f / attackRate);
        isAttacking = false;        
    }

    public void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // Y축 고정 (수평 회전만 원할 때)

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float rotationSpeed = 720f; // 회전 속도 (조절 가능)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void RotateEvent(RotateToTargetEvent rotateToTargetEvent)
    {
        //Debug.Log("RotateEvent 호출됨!");
        RotateTowardsTarget(rotateToTargetEvent.Target);

    }
}
