using System.Collections;
using UnityEngine;

public class ClickMarker : PoolLabel
{
    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ReturnPoolCoroutine());
    }

    IEnumerator ReturnPoolCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        ReturnPool();
    }
}
