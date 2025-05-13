using UnityEngine;

public class PlayerStatus
{
    public float moveSpeed;
}
public class PlayerController : ManagerBase
{
    private PlayerMovement playerMovement;
    private IInputHandler inputHandler;    
    private PlayerAnims playerAnims;
    private PlayerStatus playerStatus = new PlayerStatus();
    private PlayerAttack playerAttack;

    [SerializeField]
    Transform targetTrans;
    [SerializeField]
    bool isTargetting = false;

    ClickReturn inputReturn;
    private void Awake()
    {
        TryGetComponent<PlayerMovement>(out playerMovement);        
        TryGetComponent<PlayerAnims>(out playerAnims);
        TryGetComponent<PlayerAttack>(out playerAttack);
        playerMovement.moveAnims += playerAnims.MoveAnims;
        playerMovement.runAnims += playerAnims.RunAnims;
        playerAttack.OnStopMove += playerMovement.StopMove;
    }

    private void PlayerMovement_runAnims(bool obj)
    {
        throw new System.NotImplementedException();
    }

    public void CurrentInputHandler(IInputHandler curHandler)
    {
        inputHandler = curHandler;
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

        inputReturn = inputHandler.GetInputClick();
        if (inputReturn.pos != Vector3.zero)
        {
            playerMovement.ChangeMoveSpeed(2f);
            playerMovement.SetDestination(inputReturn.pos);
            targetTrans = null;
            isTargetting = false;
        }        
        if (inputReturn.targetTrans != null)
        {
            targetTrans = inputReturn.targetTrans;
            Debug.Log($"{targetTrans.position} , {targetTrans.name}");            
            isTargetting = true;
        }
        if(isTargetting && targetTrans !=null)
        {
            playerMovement.SetDestination(targetTrans.position);
            playerMovement.ChangeMoveSpeed(5f);
        }      

        playerMovement.MovingCheck();
        playerMovement.TargetMoving(isTargetting);
      
    }

    public override void StopGame()
    {
        base.StopGame();
        playerMovement?.StopMove();
    }
}
