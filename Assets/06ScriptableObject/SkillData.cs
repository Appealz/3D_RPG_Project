using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    public SkillType skillType;
    public float damage;
    public float coolTime;
    public float mpCost;

    //[Tooltip("Resources 폴더 내 스킬 프리팹 경로 설정 (예: 'Skills/Fireball')")]
    //public string prefabPath;

    ///// <summary>
    ///// 스킬 프리팹을 Resources 폴더에서 로드합니다.
    ///// </summary>
    //public GameObject LoadSkillPrefab()
    //{
    //    GameObject prefab = Resources.Load<GameObject>(prefabPath);
    //    if (prefab == null)
    //    {
    //        Debug.LogError($"Skill prefab not found at path: {prefabPath}");
    //    }
    //    return prefab;
    //}

    public GameObject obj;
    public ISkill GetInterface()
    {
        ISkill skill;
        obj.TryGetComponent<ISkill>(out skill);

        return skill;
    }
}
