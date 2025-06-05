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
        SetRandomDestination();
    }


    public override void StateUpdate()
    {
        // 경로 계산이 끝났고, 남은 거리가 도착거리이내 이며, 더이상 갈길이 없거나 이동속도가 0일때
        if (!Enemy.Agent.pathPending && Enemy.Agent.remainingDistance <= Enemy.Agent.stoppingDistance && (!Enemy.Agent.hasPath || Enemy.Agent.velocity.sqrMagnitude == 0f))
        {
            SetRandomDestination();
        }

        if(Vector3.Distance(Enemy.transform.position, Enemy.Target.position) <= Enemy.DetectRange)
        {
            EnemyAI.ChangeState(EnemyAI.chaseState);
        }
    }

    private void SetRandomDestination()
    {
        int findDestinationCount = 10;
        for(int i = 0; i < findDestinationCount; i++)
        {
            randomPosX = Random.Range(-3f, 3f);
            randomPosZ = Random.Range(-3f, 3f);            

            Vector3 randomPoint = Enemy.SpawnPoint.position + new Vector3(randomPosX, 0f, randomPosZ);
            
            if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                //Debug.Log($"Agent Setting Destination: {hit.position}");
                //Debug.Log($"Agent Speed: {Enemy.Agent.speed}, IsOnNavMesh: {Enemy.Agent.isOnNavMesh}, PathPending: {Enemy.Agent.pathPending}");
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
    }

}
