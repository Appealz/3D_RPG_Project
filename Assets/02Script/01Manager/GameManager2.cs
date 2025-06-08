using UnityEngine;

public class GameManager2 : DontDestroySingleton<GameManager>
{
    private PlayerController playerController;
    private CursorManager cursorManager;
    private IInputHandler curInputHandler;
    
    private DataManager dataManager;
    Player player;
    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
        curInputHandler = GameObject.FindAnyObjectByType<PCInputManager>();
        cursorManager = GameObject.FindAnyObjectByType<CursorManager>();
        playerController?.CurrentInputHandler(curInputHandler);
        
    }

    private void Start()
    {
        playerController?.StartGame();
        
        player?.StartGame();
    }

    private void Update()
    {
        playerController?.CustomUpdate();
        curInputHandler?.CustomUpdate();
        cursorManager?.CustomUpdate();
        player?.CustomUpdate();
    }

    public void StopGame()
    {
        playerController?.StopGame();
    }
}
