using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 destination;
    private NavMeshAgent agent;

    public event Action<float> moveAnims;
    public event Action<bool> runAnims;

    private void Awake()
    {
        if(!TryGetComponent<Rigidbody>(out rb))
        {
            Debug.Log("PlayerMovement.cs - Awake() - rb is not ref");
        }
        if(!TryGetComponent<NavMeshAgent>(out agent))
        {
            Debug.Log("PlayerMovement.cs - Awake() - agent is not ref");
        }
    }

    public void InitMove(float newSpeed)
    {        
        SetEnable(true);        
        agent.speed = newSpeed;
        agent.angularSpeed = 999f;
    }
    public void SetEnable(bool newEnable)
    {
        agent.enabled = newEnable;
    }

    public void StartMove()
    {
        SetEnable(true);
    }

    public void StopMove()
    {
        SetEnable(false);
    }

    public void SetDestination(Vector3 dest)
    {        
        if (agent.enabled)
        {
            agent.SetDestination(dest);
        }
    }

    public void ChangeMoveSpeed(float newSpeed)
    {
        agent.speed = newSpeed;
    }



    public void WalkAnims()
    {
        moveAnims?.Invoke(agent.velocity.sqrMagnitude);
    }

    public void RunAnims(bool isOn)
    {
        runAnims?.Invoke(isOn);
    }
}
