using UnityEngine;

public abstract class EnemyState
{
    protected Enemy Enemy;
    protected EnemyAI EnemyAI;

    public EnemyState(Enemy enemy, EnemyAI enemyAi)
    {
        Enemy = enemy;
        EnemyAI = enemyAi;
    }

    public abstract void StateEnter();
    public abstract void StateUpdate();
    public abstract void StateExit();
}
