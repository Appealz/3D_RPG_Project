using UnityEngine;

public class Projectile : PoolLabel
{
    Rigidbody rb;
    Transform target;
    Vector3 moveDir;
    GameObject Owner;
    float damage;

    bool isMove;
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
            moveDir = (target.position - transform.position).normalized;
            Move(moveDir);
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
        }
    }

    //public void SetOwner(GameObject myOwner)
    //{
    //    Owner = myOwner;
    //}

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
        if(other.CompareTag("Enemy") && other.gameObject == target.gameObject)
        {            
            Damage_Event.TakeDamage(new DamageInfo(Owner, target.gameObject, damage));
            ReturnPool();
        }
    }
}
