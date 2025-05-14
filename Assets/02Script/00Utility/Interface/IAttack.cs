using UnityEngine;

public interface IAttack
{
    void SetEnable(bool newEnable);
    void Attack(Transform targetTrans);

}
