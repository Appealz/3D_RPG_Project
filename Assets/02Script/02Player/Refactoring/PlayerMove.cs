using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviour
{
    private Vector3 targetPosition;
    
    private Transform targetTrans;
    private NavMeshAgent agent;    
    private bool OnTarget;    
    private float rotateSpeed;    
    private float moveSpeed;

    public bool isOnSkill;

    private void Awake()
    {
        if (!TryGetComponent<NavMeshAgent>(out agent))
        {
            Debug.Log("PlayerMovement.cs - Awake() - agent is not ref");
        }
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }
    private PlayerAnims anims;
    public void InitMove(float newSpeed, PlayerAnims newAnims)
    {
        anims = newAnims;

        agent.enabled = true;
        agent.acceleration = 999f;
        agent.speed = newSpeed;
        agent.updateRotation = false;
        rotateSpeed = 12f;
        agent.autoBraking = false;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }


    public void StartMove(float newSpeed)
    {
        agent.speed = newSpeed;
        agent.isStopped = false;
    }

    public void StopMove()
    {
        //SetEnable(false);
        //WalkAnims(false);
        //RunAnims(false);
        agent.isStopped = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        anims.MoveAnims(false);
        anims.RunAnims(false);
    }

    public void SettingPoisition(Vector3 newTargetPos)
    {
        targetPosition = newTargetPos;
    }

    public void SettingTarget(Transform newTargetTrans)
    {
        targetTrans = newTargetTrans;
    }

    public void Move()
    {
        if (agent.enabled && !isOnSkill)
        {
            anims.RunAnims(false);
            anims.MoveAnims(true);
            agent.SetDestination(targetPosition);


            ManualRotate(agent.desiredVelocity);

            if (agent.velocity.sqrMagnitude < 0.001f)
            {
                anims.MoveAnims(false);
            }
        }
        else if(isOnSkill)
        {
            anims.RunAnims(true);            
            agent.SetDestination(targetPosition);


            ManualRotate(agent.desiredVelocity);

            if (agent.velocity.sqrMagnitude < 0.001f)
            {
                anims.MoveAnims(false);
            }
        }
    }

    public void Chase()
    {
        if(agent.enabled)
        {
            anims.RunAnims(true);
            agent.SetDestination(targetTrans.position);

            ManualRotate(agent.desiredVelocity);
        }        
    }

    public bool IsInRange(float range)
    {
        if (targetTrans == null)        
            return false;
        float distSqr = (targetTrans.position - transform.position).sqrMagnitude;
        return distSqr <= range;
    }

    public bool IsInRangePosition(float range)
    {
        float distSqr = (targetPosition - transform.position).sqrMagnitude;
        return distSqr <= range;
    }


    public void ManualRotate(Vector3 direction)
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

    public void RotateEvent(RotateToPosEvent rotateToPosEvent)
    {
        Vector3 dir = rotateToPosEvent.Position - transform.position;
        ManualRotate(dir.normalized);
    }


}
