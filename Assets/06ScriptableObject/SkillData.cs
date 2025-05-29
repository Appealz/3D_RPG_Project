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



    //public GameObject obj;

    //[Tooltip("Resources 폴더 내 스킬 프리팹 경로 설정 (예: 'Skills/Fireball')")]
    //public string prefabPath;

    //ISkill skillinter;
    ///// <summary>
    ///// 스킬 프리팹을 Resources 폴더에서 로드합니다.
    ///// </summary>
    //public void LoadSkillPrefab()
    //{
    //    obj = Resources.Load<GameObject>(prefabPath);
    //    if (obj == null)
    //    {
    //        Debug.LogError($"Skill prefab not found at path: {prefabPath}");
    //    }      
    //}

    //public ISkill GetInterface()
    //{
    //    ISkill skill;
    //    obj.TryGetComponent<ISkill>(out skill);

    //    return skill;
    //}

    //public ISkill returnInterface(SkillType skillType)
    //{        
    //    switch(skillType)
    //    {
    //        case SkillType.Q_Skill:
    //            skillinter = new Q_Skill();
    //            break;
    //        case SkillType.W_Skill:
    //            skillinter = new W_Skill();
    //            break;
    //        case SkillType.E_Skill:
    //            skillinter = new E_Skill();
    //            break;
    //        case SkillType.R_Skill:
    //            skillinter = new R_Skill();
    //            break;
    //    }

    //    return skillinter;
    //}
}
