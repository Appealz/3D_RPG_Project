using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(Enemy enemy, EnemyAI enemyAi) : base(enemy, enemyAi)
    {
    }


    public override void StateEnter()
    {
        Debug.Log($"{Enemy.name}플레이어 추적 시작");
        Enemy.Agent.SetDestination(Enemy.Target.position);
    }
        
    public override void StateUpdate()
    {
        Enemy.Agent.SetDestination(Enemy.Target.position);
        if (Vector3.Distance(Enemy.transform.position, Enemy.Target.position) < Enemy.AttackRange)
        {
            EnemyAI.ChangeState(EnemyAI.attackState);
        }

        if(Vector3.Distance(Enemy.transform.position, Enemy.Target.position) > Enemy.DetectRange)
        {
            EnemyAI.ChangeState(EnemyAI.returnState);
        }
    }

    public override void StateExit()
    {
        Enemy.Agent.ResetPath();
    }

}
