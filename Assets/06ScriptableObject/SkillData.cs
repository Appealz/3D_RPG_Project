using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public SkillType skillType;
    public StateType stateType;
    public float damage;
    public float coolTime;
    public float mpCost;
    public float range;    
}
