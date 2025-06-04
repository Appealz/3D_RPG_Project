using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

[RequireComponent(typeof(NavMeshAgent))]
public class MoveTest : PoolLabel
{
    NavMeshAgent agent;
    // 60~ 70 , 0.6 , 20 ~ 23
    float randomPosX;
    float randomPosZ;

    Vector3 destPos;

    float maxHp;
    float curHp;

    GameObject obj;

    [SerializeField]
    private Material outlineMat;
    private Material originMat;

    private SkinnedMeshRenderer meshRenderer;

    private UnitHUD hud;
    private void Awake()
    {
        TryGetComponent<NavMeshAgent>(out agent);
        agent.enabled = true;
        agent.speed = 3.0f;
        randomPosX = Random.Range(60f, 120f);
        randomPosZ = Random.Range(20f, 30f);
        destPos = new Vector3(randomPosX, 0.6f, randomPosZ);
        SetDest(destPos);

        maxHp = 100f;
        curHp = 100f;

        meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        originMat = meshRenderer.material;

    }

    private void OnEnable()
    {        
        Damage_Event.OnDamageChange += Handle_TakeDamaged;
        obj = ObjectPoolManager.Instance.pool[6].PopObj();
        obj.TryGetComponent<UnitHUD>(out hud);
        hud.SetTarget(transform);
    }

    private void OnDisable()
    {
        Damage_Event.OnDamageChange -= Handle_TakeDamaged;
        hud.ReturnPool();
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

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {
            if(hit.transform == transform)
            {
                OnEnableOutline();
            }
            
        }
        else
        {
            DisableOutline();
        }
    }

    private void OnEnableOutline()
    {
        meshRenderer.material = outlineMat;
    }

    private void DisableOutline()
    {
        meshRenderer.material = originMat;
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
            curHp -= damageInfo.damage;
            EventBus.Publish(new HpChangeEvent(gameObject, curHp, maxHp));
        }

        if(curHp <= 0f)
        {
            //obj.GetComponent<UnitHUD>().ReturnPool();
            //ReturnPool();
            //Destroy(gameObject);
            StartCoroutine(ReturnPoolCor());
        }
    }

    IEnumerator ReturnPoolCor()
    {
        yield return null;
        
        yield return new WaitForSeconds(0.5f);
        ReturnPool();
    }
    
}
