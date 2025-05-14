using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour, IAttack
{
    public event Action OnStopMove;
    public event Action OnAttackAnims;

    private bool isAttack;

    

    public void Attack(Transform targetTrans)
    {   
        if(isAttack)
        {
            OnStopMove?.Invoke();
            OnAttackAnims?.Invoke();
            Debug.Log($"АјАн : {targetTrans.name}, {targetTrans}");
        }
    }

    public void SetEnable(bool newEnable)
    {
        isAttack = newEnable;
    }



}
