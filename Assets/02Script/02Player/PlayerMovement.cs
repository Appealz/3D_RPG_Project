using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Editor;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 destination;
    private Transform target;
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


        PCInputManager.OnMoveEvent += SetPosition;
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

    public void SetTarget(Transform transform)
    {
        SetEnable(true);
        target = transform;
    }
    public void SetPosition(Vector3 vector)
    {        
        destination = vector;
        SetEnable(true);
    }

    public void Move()
    {        
        if (agent.enabled)
        {
            agent.SetDestination(destination);
        }
        if (agent.velocity.sqrMagnitude <= 0.1f)
        {
            SetEnable(false);
        }
    }

    public void ChangeMoveSpeed(float newSpeed)
    {
        agent.speed = newSpeed;
    }

    #region _Anims_

    public void WalkAnims()
    {
        moveAnims?.Invoke(agent.velocity.sqrMagnitude);
    }

    public void RunAnims(bool isOn)
    {
        runAnims?.Invoke(isOn);
    }
    #endregion
}
