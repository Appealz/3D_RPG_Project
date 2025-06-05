using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{    
    public EnemyType Type;
    public float maxHP;    
    public float moveSpeed;
    public float attackRange;
    public float detectRange;
    
}
