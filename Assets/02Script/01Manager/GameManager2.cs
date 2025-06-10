using Unity.VisualScripting;
using UnityEngine;

public class GameManager2 : DontDestroySingleton<GameManager>
{
    private PlayerController playerController;
    private CursorManager cursorManager;
    private IInputHandle curInputHandler;
    private UIManager uiManager;
    private DataManager2 dataManager;
    Player player;
    private PlayerActionController playerActionController;

    
    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        //playerController = GameObject.FindAnyObjectByType<PlayerController>();
        curInputHandler = GameObject.FindAnyObjectByType<InputManager>();
        cursorManager = GameObject.FindAnyObjectByType<CursorManager>();
        //playerController?.CurrentInputHandler(curInputHandler);
        playerActionController = GameObject.FindAnyObjectByType<PlayerActionController>();
        uiManager = GameObject.FindAnyObjectByType<UIManager>();
        dataManager = GameObject.FindAnyObjectByType<DataManager2>();

        //SetupSkills();
        playerActionController?.BindToInputHandler(curInputHandler, player);


    }

    private void Start()
    {
        var (Q, W, E, R) = dataManager.GetAllSkillData();

        SkillSetting(SkillFactory.CreateSkill(Q), KeyCode.Q);
        SkillSetting(SkillFactory.CreateSkill(W), KeyCode.W);
        SkillSetting(SkillFactory.CreateSkill(E), KeyCode.E);
        SkillSetting(SkillFactory.CreateSkill(R), KeyCode.R);

        playerController?.StartGame();
        uiManager?.StartGame();
        player?.StartGame();


    }

    private void Update()
    {
        playerController?.CustomUpdate();
        //curInputHandler?.CustomUpdate();
        cursorManager?.CustomUpdate();
        playerActionController?.CustomUpdate();
        player?.CustomUpdate();
    }

    public void StopGame()
    {
        playerController?.StopGame();
    }


    private void SkillSetting(ISkill newSkill, KeyCode newKey)
    {        
        player.RegistSkill(newSkill);
        curInputHandler.BindKeyToSkill(newKey, newSkill.myType);
    }
}
