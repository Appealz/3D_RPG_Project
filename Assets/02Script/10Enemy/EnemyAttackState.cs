using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, EnemyAI enemyAi) : base(enemy, enemyAi)
    {

    }

    bool isAttack = false;
    bool isAttacking = false;
    float attackRate = 3f;
    float attackTime = 0f;

    float animsTIme = 0f;
    float animsDuration = 2f;
    public override void StateEnter()
    {
        Enemy.Agent.ResetPath();
        Enemy.Agent.velocity = Vector3.zero;
        isAttack = true;
    }

    public override void StateUpdate()
    {
        float TargetDistance = Vector3.Distance(Enemy.transform.position, Enemy.Target.position);

        if (isAttacking)
        {
            if (Time.time >= animsTIme)
            {
                Debug.Log("애니메이션 끝");
                isAttacking = false;
            }
            else
            {
                return;
            }
            
        }

        if (TargetDistance > Enemy.DetectRange)
        {
            EnemyAI.ChangeState(EnemyAI.returnState);
        }

        else if (TargetDistance > Enemy.AttackRange)
        {
            EnemyAI.ChangeState(EnemyAI.chaseState);
        }

        Attack();
    }


    public override void StateExit()
    {
        isAttack = false;
    }

    private void Attack()
    {
        if(!isAttack)
        {
            return;
        }
        
        if (Time.time >= attackTime)
        {
            isAttacking = true;
            Debug.Log("플레이어 공격");
            Damage_Event.TakeDamage(new DamageInfo(Enemy.gameObject, Enemy.Target.gameObject, 5f));
            attackTime = Time.time + attackRate;
            Debug.Log("애니메이션 시작");
            animsTIme = Time.time + animsDuration;
        }
    }
}
