using UnityEngine;

public class GameManager : DontDestroySingleton<GameManager>
{
    private PlayerController playerController;
    private CursorManager cursorManager;
    private IInputHandler curInputHandler;

    private void Awake()
    {
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
        curInputHandler = GameObject.FindAnyObjectByType<PCInputManager>();
        cursorManager = GameObject.FindAnyObjectByType<CursorManager>();
        playerController?.CurrentInputHandler(curInputHandler);
    }

    private void Start()
    {
        playerController?.StartGame();
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
