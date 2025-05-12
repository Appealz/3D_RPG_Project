using UnityEngine;

public class PlayerStatus
{
    public float moveSpeed;
}
public class PlayerController : ManagerBase
{
    private PlayerMovement playerMovement;
    private PlayerInputHandler playerInputHandler;
    private PlayerAnims playerAnims;
    private PlayerStatus playerStatus = new PlayerStatus();
    private void Awake()
    {
        TryGetComponent<PlayerMovement>(out playerMovement);
        TryGetComponent<PlayerInputHandler>(out  playerInputHandler);
        TryGetComponent<PlayerAnims>(out playerAnims);

        playerMovement.moveAnims += playerAnims.MoveAnims;
    }

    public override void StartGame()
    {
        base.StartGame();
        playerStatus.moveSpeed = 2f;
        playerMovement.InitMove(playerStatus.moveSpeed);
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if(Input.GetMouseButtonDown(1))
        {
            Vector3 destination = playerInputHandler.GetInputMousePosition();
            if (destination != Vector3.zero)
            {
                playerMovement.SetDestination(destination);
            }
        }
        playerMovement.MovingCheck();
    }

    public override void StopGame()
    {
        base.StopGame();
        playerMovement?.StopMove();
    }
}
