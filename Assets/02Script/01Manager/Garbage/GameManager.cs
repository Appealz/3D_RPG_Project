using UnityEngine;

public class GameManager : DontDestroySingleton<GameManager>
{
    private PlayerController playerController;
    private CursorManager cursorManager;
    private IInputHandler curInputHandler;
    private UIManager uiManager;
    private DataManager dataManager;

    private void Awake()
    {
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
        curInputHandler = GameObject.FindAnyObjectByType<PCInputManager>();
        cursorManager = GameObject.FindAnyObjectByType<CursorManager>();
        playerController?.CurrentInputHandler(curInputHandler);
        uiManager = GameObject.FindAnyObjectByType<UIManager>();
        dataManager = GameObject.FindAnyObjectByType<DataManager>();

        var (q, w, e, r) = dataManager.GetAllSkillData();
        uiManager.SetSkillData(q, w, e, r);
    }

    private void Start()
    {
        playerController?.StartGame();
        uiManager?.StartGame();
    }

    private void Update()
    {
        playerController?.CustomUpdate();
        curInputHandler?.CustomUpdate();
        cursorManager?.CustomUpdate();
    }

    public void StopGame()
    {
        playerController?.StopGame();
    }


}