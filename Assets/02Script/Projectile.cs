using UnityEngine;

public class Projectile : PoolLabel
{
    Rigidbody rb;
    Transform target;
    Vector3 moveDir;
    GameObject Owner;
    float damage;

    bool isMove;

    Vector3 lastTargetPos;
    private void Awake()
    {
        if(!TryGetComponent<Rigidbody>(out rb))
        {
            Debug.Log($"{gameObject.name} : Proejctile.cs - Awake() - rb is not ref");
        }
        isMove = false;
    }

    private void OnEnable()
    {
        Skill_Event.ProjectileSpawned += SettingInfo;
    }

    private void OnDisable()
    {
        Skill_Event.ProjectileSpawned -= SettingInfo;
        isMove = false;
    }

    private void Update()
    {        
        if(isMove)
        {
            //if (target == null)
            //{
            //    ReturnPool();
            //}

            //moveDir = (target.position - transform.position).normalized;
            //Move(moveDir);

            if (target == null)
            {
                // 타겟이 없으면 마지막 위치까지 이동
                moveDir = (lastTargetPos - transform.position).normalized;
                Move(moveDir);

                // 마지막 위치 근처에 도착했으면 리턴
                if (Vector3.Distance(transform.position, lastTargetPos) <= 0.1f)
                {
                    ReturnPool();
                }
            }
            else
            {
                lastTargetPos = target.position; // 타겟 위치 저장
                moveDir = (target.position - transform.position).normalized;
                Move(moveDir);
            }
        }
    }

    public void SettingInfo(ProjectileInfo projInfo)
    {
        if(projInfo.myType == ProjectileType.Normal)
        {            
            Owner = projInfo.owner;
            damage = projInfo.damage;
            TargetSetting(projInfo.target);
            SetEnable(true);
            Skill_Event.ProjectileSpawned -= SettingInfo;
        }
    }

    public void TargetSetting(Transform targetTrans)
    {
        target = targetTrans;
        moveDir = (targetTrans.position - transform.position).normalized;
    }

    public void Move(Vector3 dir)
    {
        rb.linearVelocity = dir * 10f;
    }

    public void SetEnable(bool newEnable)
    {
        isMove = newEnable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy") && other.gameObject == target.gameObject)
        {            
            Damage_Event.TakeDamage(new DamageInfo(Owner, target.gameObject, damage));
            ReturnPool();
        }
    }
}
