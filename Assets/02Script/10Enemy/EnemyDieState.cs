using System.Collections;
using UnityEngine;

public class EnemyDieState : EnemyState
{
    public EnemyDieState(Enemy enemy, EnemyAI enemyAi) : base(enemy, enemyAi)
    {

    }

    public override void StateEnter()
    {

        Enemy.StartCoroutine(ReturnPoolCoroutine());
    }

    public override void StateExit()
    {
        
    }

    public override void StateUpdate()
    {
        
    }

    IEnumerator ReturnPoolCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        Enemy.ReturnPool();
    }

}
