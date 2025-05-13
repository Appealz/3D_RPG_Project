using UnityEngine;

public class GameManager : DontDestroySingleton<GameManager>
{
    private PlayerController playerController;
    private IInputHandler curInputHandler;

    private void Awake()
    {
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
        curInputHandler = GameObject.FindAnyObjectByType<PCInputManager>();
    }

    private void Start()
    {
        playerController?.CurrentInputHandler(curInputHandler);
    }

    private void Update()
    {
        playerController?.CustomUpdate();        
    }

    public void StopGame()
    {
        playerController?.StopGame();
    }
}
