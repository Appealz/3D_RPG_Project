using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    public float moveSpeed;
    public float runSpeed;
    public float attackRate;
    public float attackRagne;
    public float maxMp;    
    public float maxHp;
    public float attackDamage;
}
