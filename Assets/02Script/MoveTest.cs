using UnityEngine;
using UnityEngine.AI;

public class MoveTest : MonoBehaviour
{
    NavMeshAgent agent;
    // 60~ 70 , 0.6 , 20 ~ 23
    float randomPosX;
    float randomPosZ;

    Vector3 destPos;
    private void Awake()
    {
        TryGetComponent<NavMeshAgent>(out agent);
        agent.enabled = true;
        agent.speed = 3.0f;
        randomPosX = Random.Range(60f, 120f);
        randomPosZ = Random.Range(20f, 30f);
        destPos = new Vector3(randomPosX, 0.6f, randomPosZ);
        SetDest(destPos);
    }

    private void OnEnable()
    {
        Damage_Event.OnDamageChange += Handle_TakeDamaged;
    }

    private void Update()
    {
        if(agent.velocity.sqrMagnitude <= 0)
        {
            randomPosX = Random.Range(60f, 120f);
            randomPosZ = Random.Range(20f, 30f);
            destPos = new Vector3(randomPosX, 0.6f, randomPosZ);
            SetDest(destPos);
        }
    }

    private void SetDest(Vector3 dest)
    {
        agent.SetDestination(dest);
    }

    public void Handle_TakeDamaged(DamageInfo damageInfo)
    {
        if(damageInfo.defender == gameObject)
        {
            Debug.Log($"{damageInfo.attacker.name}의 공격, {damageInfo.damage} 피해 입음");
        }
    }

    
}
