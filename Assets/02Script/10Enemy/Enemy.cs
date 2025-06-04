using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

public enum EnemyStateType
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Die
}

public class EnemyStatus
{
    public float maxHP = 100f;
    public float curHP = 100f;
    public float moveSpeed;
    
}

public class Enemy : PoolLabel
{
    EnemyAI enemyAI;
    NavMeshAgent agent;
    Transform target;
    EnemyStatus enemyStatus;
    Transform spawnPoint;

    public EnemyAI EnemyAI => enemyAI;
    public NavMeshAgent Agent => agent;
    public Transform Target => target;
    public EnemyStatus EnemyStatus => enemyStatus;
    public Transform SpawnPoint => spawnPoint;

    UnitHUD hud;    

    


    private void Awake()
    {
        // 스탯
        enemyStatus = new EnemyStatus();

        // 타겟
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }

        // NavMesh
        if(!TryGetComponent<NavMeshAgent>(out agent))
        {
            Debug.Log("agent 참조실패");
        }

        enemyAI = new EnemyAI(this);
            
    }

    private void OnEnable()
    {
        Damage_Event.OnDamageChange += Handle_TakeDamaged;
        GameObject obj = ObjectPoolManager.Instance.pool[6].PopObj();
        obj.TryGetComponent<UnitHUD>(out hud);
        hud.SetTarget(transform);

        // 스폰위치
        spawnPoint = transform;
    }

    private void OnDisable()
    {
        Damage_Event.OnDamageChange -= Handle_TakeDamaged;
        hud.ReturnPool();
    }

    void Update()
    {
        enemyAI.currentState?.StateUpdate();
    }



    public void Handle_TakeDamaged(DamageInfo damageInfo)
    {
        if (damageInfo.defender == gameObject)
        {
            Debug.Log($"{damageInfo.attacker.name}의 공격, {damageInfo.damage} 피해 입음");
            enemyStatus.curHP -= damageInfo.damage;
            EventBus.Publish(new HpChangeEvent(gameObject, enemyStatus.curHP, enemyStatus.maxHP));
        }

        if (enemyStatus.curHP <= 0f)
        {
            //obj.GetComponent<UnitHUD>().ReturnPool();
            //ReturnPool();
            //Destroy(gameObject);
            StartCoroutine(ReturnPoolCor());
        }
    }

    IEnumerator ReturnPoolCor()
    {
        yield return null;

        yield return new WaitForSeconds(0.5f);
        ReturnPool();
    }
}
