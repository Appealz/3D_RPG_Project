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
        Enemy.Agent.speed= 3f;
        Enemy.Anims.PlayRun(true);
        Enemy.SetAggresive();
    }
        
    public override void StateUpdate()
    {
        Enemy.Agent.SetDestination(Enemy.Target.position);

        float TargetDistance = Vector3.Distance(Enemy.transform.position, Enemy.Target.position);
        float returnDistance = Vector3.Distance(Enemy.SpawnPoint, Enemy.transform.position);
                
        if (TargetDistance < Enemy.Status.attackRange)
        {
            EnemyAI.ChangeState(EnemyAI.attackState);
        }
        else if(!Enemy.IsProvoked && (TargetDistance > Enemy.Status.detectRange || returnDistance > 10f))
        {
            EnemyAI.ChangeState(EnemyAI.returnState);            
        }
    }

    public override void StateExit()
    {
        Enemy.Agent.ResetPath();
        Enemy.Anims.PlayRun(false);
    }

}
