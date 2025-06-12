using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private SkillData Qskill;
    [SerializeField] private SkillData Wskill;
    [SerializeField] private SkillData Eskill;
    [SerializeField] private SkillData Rskill;

    TargetSkill q_Skill;
    NonTargetSkill w_Skill;
    BarrierSkill e_Skill;
    AreaSkill r_Skill;

    PlayerController playerController;
    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        q_Skill = new TargetSkill();
        w_Skill = new NonTargetSkill();
        e_Skill = new BarrierSkill();
        r_Skill = new AreaSkill();
    }

    void Start()
    {
        q_Skill.SetupData(Qskill);
        q_Skill.SetOwner(playerController.gameObject);
        w_Skill.SetupData(Wskill);
        w_Skill.SetOwner(playerController.gameObject);
        e_Skill.SetupData(Eskill);
        e_Skill.SetOwner(playerController.gameObject);
        r_Skill.SetupData(Rskill);
        r_Skill.SetOwner(playerController.gameObject);

        playerController.RegistSkill(KeyCode.Q, q_Skill);
        playerController.RegistSkill(KeyCode.W, w_Skill);
        playerController.RegistSkill(KeyCode.E, e_Skill);
        playerController.RegistSkill(KeyCode.R, r_Skill);
    }


    private void OnDisable()
    {
        playerController.ReleaseSkill(q_Skill);
        playerController.ReleaseSkill(w_Skill);
        playerController.ReleaseSkill(e_Skill);
        playerController.ReleaseSkill(r_Skill);
    }
    //private void SkillInit(ISkill skill, SkillData skillData)
    //{
    //    skill.Init(skillData);
    //}

    public (SkillData Q, SkillData W, SkillData E, SkillData R) GetAllSkillData()
    {
        return (Qskill, Wskill, Eskill, Rskill);
    }
}