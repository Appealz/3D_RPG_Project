using UnityEngine;

public class UIManager : ManagerBase
{
    #region _Field_
    [SerializeField]
    GameObject owner;
    PlayerHUD playerHUD;

    [Header("Skill Data")]
    [SerializeField] private SkillData Qskill;
    [SerializeField] private SkillData Wskill;
    [SerializeField] private SkillData Eskill;
    [SerializeField] private SkillData Rskill;
    #endregion

    private void Awake()
    {
        GameObject.Find("PlayerUI").TryGetComponent<PlayerHUD>(out playerHUD);

        if (playerHUD == null)
        {
            Debug.LogError("playerHUD가 할당되지 않았습니다.");
        }
    }

    public override void StartGame()
    {
        base.StartGame();
        playerHUD?.SetSkillMpMarkers(Qskill, Wskill, Eskill, Rskill);
        playerHUD?.SetOwner(owner);
    }

    public void SetPlayer(GameObject newPlayer)
    {
        owner = newPlayer;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void SetSkillData(SkillData q, SkillData w, SkillData e, SkillData r)
    {
        Qskill = q;
        Wskill = w;
        Eskill = e;
        Rskill = r;
    }

 

    public override void CustomUpdate()
    {
        base.CustomUpdate();
    }
}
