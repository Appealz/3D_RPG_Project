using UnityEngine;

public class EnemyAI
{
    private Enemy Enemy;

    public EnemyIdleState idleState;
    public EnemyAttackState attackState;
    public EnemyPatrolState patrolState;
    public EnemyChaseState chaseState;
    public EnemyDieState dieState;
    public EnemyReturnState returnState;

    public EnemyAI(Enemy enemy)
    {
        Enemy = enemy;

        idleState = new EnemyIdleState(Enemy, this);
        attackState = new EnemyAttackState(Enemy, this);
        patrolState = new EnemyPatrolState(Enemy, this);
        chaseState = new EnemyChaseState(Enemy, this);
        dieState = new EnemyDieState(Enemy, this);
        returnState = new EnemyReturnState(Enemy, this);
    }


    public EnemyState currentState;

    public void ChangeState(EnemyState newEnemyState)
    {
        if(currentState != null)
        {
            currentState.StateExit();
        }

        currentState = newEnemyState;

        if(currentState != null)
        {
            currentState.StateEnter();
        }
    }

    public void Handle_OnDie()
    {
        Debug.Log("dieState ÀüÈ¯");
        ChangeState(dieState);
    }
}
