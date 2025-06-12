using UnityEngine;

public class DataManager2 : ManagerBase
{
    [SerializeField] private SkillData Qskill;
    [SerializeField] private SkillData Wskill;
    [SerializeField] private SkillData Eskill;
    [SerializeField] private SkillData Rskill;
    [SerializeField] private PlayerData playerData;

    public (SkillData Q, SkillData W, SkillData E, SkillData R) GetAllSkillData()
    {
        return (Qskill, Wskill, Eskill, Rskill);
    }

    public PlayerData GetPlayerData()
    {
        return playerData;
    }

    private void Awake()
    {
       
    }
    void Start()
    {
        
    }


    private void OnDisable()
    {
       
    }  


}
