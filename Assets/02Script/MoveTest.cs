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
        agent.speed = 1.0f;
        randomPosX = Random.Range(60f, 120f);
        randomPosZ = Random.Range(20f, 30f);
        destPos = new Vector3(randomPosX, 0.6f, randomPosZ);
        SetDest(destPos);
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

    
}
