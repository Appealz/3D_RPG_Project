using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private SkillData Qskill;
    [SerializeField] private SkillData Wskill;
    [SerializeField] private SkillData Eskill;
    [SerializeField] private SkillData Rskill;

    PlayerController playerController;
    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }
        
    void Start()
    {
        SkillInit(Qskill.GetInterface(), Qskill);
        playerController.RegistSkill(KeyCode.Q, Qskill.GetInterface());

        SkillInit(Wskill.GetInterface(), Wskill);
        playerController.RegistSkill(KeyCode.W, Wskill.GetInterface());

        SkillInit(Eskill.GetInterface(), Eskill);
        playerController.RegistSkill(KeyCode.E, Eskill.GetInterface());

        SkillInit(Rskill.GetInterface(), Rskill);
        playerController.RegistSkill(KeyCode.R, Rskill.GetInterface());
    }

    private void SkillInit(ISkill skill, SkillData skillData)
    {
        skill.Init(skillData);
    }
}
