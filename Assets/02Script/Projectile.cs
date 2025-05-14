using UnityEngine;

public class Projectile : PoolLabel
{
    Rigidbody rb;
    Transform target;
    Vector3 moveDir;

    bool isMove;
    private void Awake()
    {
        TryGetComponent<Rigidbody>(out rb);
        isMove = false;
    }

    private void OnDisable()
    {
        isMove = false;
    }

    private void Update()
    {        
        if(isMove)
        {
            Move(moveDir);
        }        
    }

    public void TargetSetting(Transform targetTrans)
    {
        target = targetTrans;
        moveDir = (targetTrans.position - transform.position).normalized;
    }

    public void Move(Vector3 dir)
    {
        rb.linearVelocity = dir * 5f;
    }

    public void SetEnable(bool newEnable)
    {
        isMove = newEnable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log(other.name);
            ReturnPool();
        }        
    }
}
