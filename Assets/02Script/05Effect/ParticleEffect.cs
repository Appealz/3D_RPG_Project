using System.Collections;
using UnityEngine;

public class ParticleEffect : PoolLabel
{

    private void OnEnable()
    {
        StartCoroutine(returnPoolCor());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator returnPoolCor()
    {
        yield return new WaitForSeconds(0.5f);
        ReturnPool();
    }


}
