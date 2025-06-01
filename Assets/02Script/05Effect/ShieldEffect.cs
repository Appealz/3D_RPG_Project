using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ShieldEffect : PoolLabel
{
    GameObject owner;
    float shieldAmount;
    Transform shieldTrans;
    

    private void OnEnable()
    {        
        StartCoroutine(ShieldDuration());
        Skill_Event.ShieldSkillSpawned += SettingInfo;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        Skill_Event.ShieldSkillSpawned -= SettingInfo;
    }

    private void SettingInfo(ShieldSkillInfo skillInfo)
    {
        owner = skillInfo.owner;
        shieldTrans = FindObjectTransform.FindChildTransform(owner.transform, "ShieldPoint");
        shieldAmount = skillInfo.shieldAmount;
    }

    private void Update()
    {
        transform.position = shieldTrans.position;
    }

    IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(2f);
        ReturnPool();
    }
}
