using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private SkillData Qskill;
    [SerializeField] private SkillData Wskill;
    [SerializeField] private SkillData Eskill;
    [SerializeField] private SkillData Rskill;

    public TargetSkill q_Skill;
    public NonTargetSkill w_Skill;
    public BarrierSkill e_Skill;
    public AreaSkill r_Skill;
  
    PlayerController playerController;
    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        q_Skill = new TargetSkill(Qskill);
        w_Skill = new NonTargetSkill(Wskill);
        e_Skill = new BarrierSkill(Eskill);
        r_Skill = new AreaSkill(Rskill);
    }

    void Start()
    {        
        q_Skill.SetOwner(playerController.gameObject);        
        w_Skill.SetOwner(playerController.gameObject);        
        e_Skill.SetOwner(playerController.gameObject);        
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
