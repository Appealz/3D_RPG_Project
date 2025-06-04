using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    public EnemyPatrolState(Enemy enemy, EnemyAI enemyAi) : base(enemy, enemyAi)
    {

    }

    float randomPosX;
    float randomPosZ;

    public override void StateEnter()
    {
        randomPosX = Random.Range(60f, 120f);
        randomPosZ = Random.Range(20f, 30f);
        Enemy.Agent.SetDestination(new Vector3(Enemy.SpawnPoint.position.x + randomPosX, Enemy.SpawnPoint.transform.position.y, Enemy.SpawnPoint.position.z + randomPosZ));
    }

    public override void StateExit()
    {
        Enemy.Agent.ResetPath();
        Enemy.Agent.velocity = Vector3.zero;
    }

    public override void StateUpdate()
    {
        if (Enemy.Agent.velocity.sqrMagnitude <= 0)
        {
            randomPosX = Random.Range(60f, 120f);
            randomPosZ = Random.Range(20f, 30f);
            Enemy.Agent.SetDestination(new Vector3(Enemy.SpawnPoint.position.x + randomPosX, Enemy.SpawnPoint.transform.position.y, Enemy.SpawnPoint.position.z + randomPosZ));
        }
    }


}
