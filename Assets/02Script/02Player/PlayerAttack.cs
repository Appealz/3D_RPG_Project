using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IAttack
{
    public event Action OnStopMove;
    

    public void Attack()
    {
        throw new System.NotImplementedException();
    }

    public void SetEnable()
    {
        throw new System.NotImplementedException();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Debug.Log("Àû ¹ß°ß");
        }
    }

}
