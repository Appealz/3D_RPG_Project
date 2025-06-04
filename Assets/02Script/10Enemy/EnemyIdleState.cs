using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyAI enemyAi) : base(enemy, enemyAi)
    {
    }

    public override void StateEnter()
    {
        Enemy.Agent.ResetPath();
        Enemy.Agent.velocity = Vector3.zero;
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate()
    {
        
    }


}
