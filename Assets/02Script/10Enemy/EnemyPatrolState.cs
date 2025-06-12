using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

public class EnemyPatrolState : EnemyState
{
    public EnemyPatrolState(Enemy enemy, EnemyAI enemyAi) : base(enemy, enemyAi)
    {

    }

    float randomPosX;
    float randomPosZ;
    
    public override void StateEnter()
    {
        //Debug.Log("패트롤 시작");
        Enemy.Agent.speed = 1.5f;
        SetRandomDestination();
        Enemy.Anims.PlayMove(true);
    }

    public override void StateUpdate()
    {        
        if (!Enemy.Agent.pathPending && Enemy.Agent.remainingDistance <= Enemy.Agent.stoppingDistance && (!Enemy.Agent.hasPath || Enemy.Agent.velocity.sqrMagnitude == 0f))
        {
            SetRandomDestination();
        }
        if(Vector3.Distance(Enemy.transform.position, Enemy.Target.position) <= Enemy.Status.detectRange)
        {
            EnemyAI.ChangeState(EnemyAI.chaseState);
        }
    }

    private void SetRandomDestination()
    {
        int findDestinationCount = 10;
        for(int i = 0; i < findDestinationCount; i++)
        {
            randomPosX = Random.Range(-5f, 5f);
            randomPosZ = Random.Range(-5f, 5f);            

            Vector3 randomPoint = Enemy.SpawnPoint + new Vector3(randomPosX, 0f, randomPosZ);
            
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                Enemy.Agent.SetDestination(hit.position);
                return;                
            }
            else
            {
                Debug.Log($"{Enemy.name}이 유효한 NavMesh 위치를 찾지 못했습니다. 랜덤 포인트 다시 생성.");
            }
        }
    }

    public override void StateExit()
    {
        Enemy.Agent.ResetPath();
        Enemy.Agent.velocity = Vector3.zero;
        Enemy.Anims.PlayMove(false);
    }

}
