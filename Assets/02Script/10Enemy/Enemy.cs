using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

public enum EnemyType
{
    Melee,
    Range,
}

public class EnemyStatus
{
    public EnemyType Type;
    private float maxHP;
    public float MaxHP => maxHP;
        
    public float curHP;
    public float moveSpeed;
    public float attackRange;
    public float detectRange;

    public EnemyStatus(EnemyData enemyData)
    {
        Type = enemyData.Type;
        maxHP = enemyData.maxHP;
        curHP = enemyData.maxHP;
        moveSpeed = enemyData.moveSpeed;
        attackRange = enemyData.attackRange;
        detectRange = enemyData.detectRange;
    }


}

public class Enemy : PoolLabel
{
    EnemyAI enemyAI;
    NavMeshAgent agent;
    Transform target;
    
    Vector3 spawnPoint;
    EnemyAnimsController animController;

    [SerializeField]
    EnemyData enemyData;

    public EnemyAI EnemyAI => enemyAI;
    public NavMeshAgent Agent => agent;
    public Transform Target => target;
    private EnemyStatus enemyStatus;
    public EnemyStatus Status => enemyStatus;
    public Vector3 SpawnPoint => spawnPoint;
    public EnemyAnimsController Anims => animController;

    UnitHUD hud;

    [SerializeField]
    private EnemyType myType;

    public EnemyType MyType => myType;

    private bool isProvoked;
    public bool IsProvoked => isProvoked;
    public event Action OnDieEvent;

    private float aggroTime;
    private float aggroDuration = 5f;
    private void Awake()
    {
        // 스탯

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

        // AI
        enemyAI = new EnemyAI(this);

        // 애니메이션
        if(!TryGetComponent<EnemyAnimsController>(out animController))
        {
            Debug.Log("animController 참조 실패");
        }            
    }

    private void OnEnable()
    {
        Damage_Event.OnDamageChange += Handle_TakeDamaged;
        GameObject obj = ObjectPoolManager.Instance.pool[6].PopObj();
        obj.TryGetComponent<UnitHUD>(out hud);
        hud.SetTarget(transform);
        OnDieEvent += enemyAI.Handle_OnDie;    
    }

    public void Init(Vector3 spawnPos)
    {
        enemyStatus = new EnemyStatus(enemyData);

        spawnPoint = spawnPos;
        agent.speed = enemyStatus.moveSpeed;
        enemyAI.currentState = enemyAI.idleState;
        enemyAI.ChangeState(enemyAI.patrolState);        
    }

    private void OnDisable()
    {
        Damage_Event.OnDamageChange -= Handle_TakeDamaged;
        OnDieEvent -= enemyAI.Handle_OnDie;
        hud.ReturnPool();
    
    }

    void Update()
    {
        enemyAI.currentState?.StateUpdate();

        if(IsProvoked)
        {
            if(Time.time > aggroTime)
            {
                isProvoked = false;
            }
        }
    }


    public void SetAggresive()
    {
        aggroTime = Time.time + aggroDuration;
        isProvoked = true;
    }

    public void SetSpawnPoint(Vector3 newPoint)
    {
        spawnPoint = newPoint;
    }

    public void Handle_TakeDamaged(DamageInfo damageInfo)
    {
        if (damageInfo.defender == gameObject)
        {
            Anims.PlayHit();
            Debug.Log($"{damageInfo.attacker.name}의 공격, {damageInfo.damage} 피해 입음");
            enemyStatus.curHP -= damageInfo.damage;
            EventBus.Publish(new HpChangeEvent(gameObject, enemyStatus.curHP, enemyStatus.MaxHP));
            enemyAI.ChangeState(enemyAI.chaseState);
        }

        if (enemyStatus.curHP <= 0f && enemyAI.currentState != enemyAI.dieState)
        {
            OnDieEvent?.Invoke();            
        }
    }
       

}
