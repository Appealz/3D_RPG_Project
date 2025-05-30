using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.GridLayoutGroup;

public class FireEffect : PoolLabel
{
    Vector3 dir;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(ReturnTime());
        EventBus.Subscribe<SkillTargetPositionEvent>(TargetPos);
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        EventBus.UnSubscribe<SkillTargetPositionEvent>(TargetPos);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(dir);
    }
    private void TargetPos(SkillTargetPositionEvent targetPos)
    {
        dir = targetPos.TargetPos;
    }

    IEnumerator ReturnTime()
    {
        yield return new WaitForSeconds(1f);
        ReturnPool();
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.CompareTag("Enemy") && other.gameObject == target.gameObject)
        //{
        //    Damage_Event.TakeDamage(new DamageInfo(Owner, target.gameObject, damage));
        //    ReturnPool();
        //}
    }
}
