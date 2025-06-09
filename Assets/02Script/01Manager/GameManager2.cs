using Unity.VisualScripting;
using UnityEngine;

public class GameManager2 : DontDestroySingleton<GameManager>
{
    private PlayerController playerController;
    private CursorManager cursorManager;
    private IInputHandle curInputHandler;
    private UIManager uiManager;
    private DataManager dataManager;
    Player player;
    private PlayerActionController playerActionController;
    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
        curInputHandler = GameObject.FindAnyObjectByType<InputManager>();
        cursorManager = GameObject.FindAnyObjectByType<CursorManager>();
        //playerController?.CurrentInputHandler(curInputHandler);
        playerActionController = GameObject.FindAnyObjectByType<PlayerActionController>();
        uiManager = GameObject.FindAnyObjectByType<UIManager>();
        dataManager = GameObject.FindAnyObjectByType<DataManager>();

        //SetupSkills();
        playerActionController?.BindToInputHandler(curInputHandler, player);
    }

    private void Start()
    {
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

    //private void SetupSkills()
    //{
    //    var (q, w, e, r) = dataManager.GetAllSkillData();

    //    playerController.RegistSkill(KeyCode.Q, dataManager.q_Skill);
    //    playerController.RegistSkill(KeyCode.W, dataManager.w_Skill);
    //    playerController.RegistSkill(KeyCode.E, dataManager.e_Skill);
    //    playerController.RegistSkill(KeyCode.R, dataManager.r_Skill);

    //    uiManager.SetSkillData(q, w, e, r);
    //}

}
