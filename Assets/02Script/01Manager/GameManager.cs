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
        playerController?.StartGame();
    }

    private void Update()
    {
        playerController?.CustomUpdate();
        curInputHandler?.CustomUpdate();
    }

    public void StopGame()
    {
        playerController?.StopGame();
    }
}
