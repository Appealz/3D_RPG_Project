using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ExplosionEffect : PoolLabel
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(ReturnTime());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator ReturnTime()
    {
        yield return new WaitForSeconds(0.5f);
        ReturnPool();
    }
}
