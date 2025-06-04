using UnityEngine;

public class EnemyReturnState : EnemyState
{
    public EnemyReturnState(Enemy enemy, EnemyAI enemyAI) : base(enemy, enemyAI) { }

    public override void StateEnter()
    {
        if (Enemy.Agent.isOnNavMesh)
        {
            Debug.Log($"{Enemy.name} 스폰 위치 리턴");
            Enemy.Agent.SetDestination(Enemy.SpawnPoint.position);
        }
    }

    public override void StateUpdate()
    {
        float distance = Vector3.Distance(Enemy.transform.position, Enemy.SpawnPoint.position);

        if (distance <= Enemy.Agent.stoppingDistance && !Enemy.Agent.pathPending)
        {
            EnemyAI.ChangeState(EnemyAI.patrolState);
        }
    }

    public override void StateExit()
    {
        Enemy.Agent.ResetPath();
    }
}