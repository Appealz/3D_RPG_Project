using System;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    PlayerStatus_Fixed status;
    public event Action OnDieEvent;

    private void OnEnable()
    {
        Damage_Event.OnDamageChange += Handle_TakeDamaged;
    }

    private void OnDisable()
    {
        Damage_Event.OnDamageChange -= Handle_TakeDamaged;
    }

    public void InitStatus(PlayerStatus_Fixed newStatus)
    {
        status = newStatus;
    }

    public void Handle_TakeDamaged(DamageInfo damageInfo)
    {
        if (damageInfo.defender == gameObject)
        {
            Debug.Log($"{damageInfo.attacker.name}의 공격, {damageInfo.damage} 피해 입음");
            status.CurHp -= damageInfo.damage;
        }

        if (status.CurHp <= 0f)
        {
            OnDieEvent?.Invoke();
        }
    }
}
