using Unity.VisualScripting;
using UnityEngine;

public class GameManager2 : DontDestroySingleton<GameManager>
{    
    private CursorManager cursorManager;
    private IInputHandle curInputHandler;
    private UIManager uiManager;
    private DataManager2 dataManager;
    private Player player;
    private PlayerActionController playerActionController;
    
    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        if(player == null)        
            Debug.Log("player is not ref");
                
        curInputHandler = GameObject.FindAnyObjectByType<InputManager>();
        if (curInputHandler == null)
            Debug.Log("curInputHandler is not ref");
        cursorManager = GameObject.FindAnyObjectByType<CursorManager>();
        if (cursorManager == null)
            Debug.Log("cursorManager is not ref");
        
        playerActionController = GameObject.FindAnyObjectByType<PlayerActionController>();
        if (playerActionController == null)
            Debug.Log("playerActionController is not ref");
        uiManager = GameObject.FindAnyObjectByType<UIManager>();
        if (uiManager == null)
            Debug.Log("uiManager is not ref");
        dataManager = GameObject.FindAnyObjectByType<DataManager2>();
        if (dataManager == null)
            Debug.Log("dataManager is not ref");
    }

    private void Start()
    {
        playerActionController?.BindToInputHandler(curInputHandler, player);

        // 데이터 주입(세팅)
        var (Q, W, E, R) = dataManager.GetAllSkillData();
        uiManager.SetSkillData(Q, W, E, R);
        SkillSetting(SkillFactory.CreateSkill(Q), KeyCode.Q);
        SkillSetting(SkillFactory.CreateSkill(W), KeyCode.W);
        SkillSetting(SkillFactory.CreateSkill(E), KeyCode.E);
        SkillSetting(SkillFactory.CreateSkill(R), KeyCode.R);
        player.PlayerDataSetting(dataManager.GetPlayerData());

        
        uiManager?.StartGame();
        player?.StartGame();
    }

    private void Update()
    {   
        cursorManager?.CustomUpdate();
        playerActionController?.CustomUpdate();
        player?.CustomUpdate();
    }

    public void StopGame()
    {
        player?.StopGame();
    }

    /// <summary>
    /// inputHandler, player, skill객체 있어야 가능.
    /// </summary>
    /// <param name="newSkill"></param>
    /// <param name="newKey"></param>
    private void SkillSetting(ISkill newSkill, KeyCode newKey)
    {        
        player.RegistSkill(newSkill);
        curInputHandler.BindKeyToSkill(newKey, newSkill.myType);
    }
}
