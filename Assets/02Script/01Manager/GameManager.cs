using UnityEngine;

public class GameManager : DontDestroySingleton<GameManager>
{
    private PlayerController playerController;


    private void Awake()
    {
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
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
