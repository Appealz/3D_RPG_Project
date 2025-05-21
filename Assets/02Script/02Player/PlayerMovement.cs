using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Editor;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 destination;
    [SerializeField]
    private Transform target;
    private NavMeshAgent agent;
    PlayerStatus playerStatus;

    public event Action<bool> moveAnims;
    public event Action<bool> runAnims;

    public event Action<StateType> OnChangeState;
    [SerializeField]
    private bool OnTarget;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private float moveSpeed;

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

        PCInputManager.OnMouseMoveClick += SetPosition;
        PCInputManager.OnMouseTargetClick += SetTarget;
        playerStatus = new PlayerStatus();        
    }

    public void InitMove(float newSpeed)
    {
        agent.enabled = true;
        SetEnable(true);        
        moveSpeed = newSpeed;
        agent.speed = moveSpeed;
        agent.updateRotation = false;
        rotateSpeed = 12f;
        agent.autoBraking = false;
    }

    public void SetEnable(bool newEnable)
    {
        if (agent.enabled)
        {
            agent.isStopped = !newEnable;
            if(agent.isStopped)
            {
                agent.ResetPath();
                agent.velocity = Vector3.zero;
            }            
        }
    }

    public void StartMove()
    {        
        SetEnable(true);        
        if(target)
        {
            OnTarget = true;
        }
    }

    public void StopMove()
    {
        SetEnable(false);
        WalkAnims(false);  
        RunAnims(false);
    }


    public void SetPosition(Vector3 vector)
    {        
        OnChangeState?.Invoke(StateType.Move);
        agent.speed = moveSpeed;
        StartMove();        
        target = null;
        OnTarget = false;
        destination = vector;        
    }

    public void Move()
    {        
        if (agent.enabled)
        {            
            agent.SetDestination(destination);            
            WalkAnims(true);
            RunAnims(false);

            ManualRotate(agent.desiredVelocity);

            if (agent.velocity.sqrMagnitude < 0.001f)
            {
                WalkAnims(false);
            }            
        }    
    }


    public void SetTarget(Transform transform)
    {
        SetEnable(true);
        target = transform;
        OnTarget = true;
        agent.speed = 5f;
        StartMove();
        OnChangeState?.Invoke(StateType.Chase);
    }

    public void ChaseMove()
    {
        StartMove();
        if (agent.enabled && target)
        {
            agent.SetDestination(target.position);
            RunAnims(OnTarget);

            ManualRotate(agent.desiredVelocity);
            if ((target.position - transform.position).sqrMagnitude < playerStatus.attackRagne)
            {
                StopMove();
                OnChangeState?.Invoke(StateType.Attack);                
            }
        }
    }
    private void ManualRotate(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.deltaTime * rotateSpeed // 이 값이 클수록 빠르고 작을수록 부드러움
            );
            
        }
    }


    public void ChangeMoveSpeed(float newSpeed)
    {
        agent.speed = newSpeed;
    }

    #region _Anims_

    public void WalkAnims(bool isMoving)
    {        
        moveAnims?.Invoke(isMoving);
    }

    public void RunAnims(bool isOn)
    {
        runAnims?.Invoke(isOn);
    }
    #endregion
}
