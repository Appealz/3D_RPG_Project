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
    bool isAttacking = false;
    ClickReturn inputReturn;
    private void Awake()
    {
        TryGetComponent<PlayerMovement>(out playerMovement);        
        TryGetComponent<PlayerAnims>(out playerAnims);
        TryGetComponent<PlayerAttack>(out playerAttack);
        playerMovement.moveAnims += playerAnims.MoveAnims;
        playerMovement.runAnims += playerAnims.RunAnims;
        playerAttack.OnStopMove += playerMovement.StopMove;
        playerAttack.OnAttackAnims += playerAnims.AttackAnims;
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

        Moving(inputReturn);        

        AttackState();
    }
    
    private void Moving(ClickReturn inputReturn)
    {
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
            isTargetting = true;
        }

        if (isTargetting && targetTrans != null)
        {
            playerMovement.SetDestination(targetTrans.position);
            playerMovement.ChangeMoveSpeed(5f);
        }
        playerMovement.TargetMoving(isTargetting);
        playerMovement.MovingCheck();
    }


    private void AttackState()
    {
        if (isTargetting && (targetTrans.position - transform.position).sqrMagnitude < 25f)
        {
            isAttacking = true;
            playerAttack.SetEnable(isAttacking);
            playerAttack.Attack(targetTrans);
        }
        else if (isTargetting && (targetTrans.position - transform.position).sqrMagnitude >= 25f)
        {
            isAttacking = false;
            playerAttack.SetEnable(isAttacking);
            if(!isAttacking)
            {
                playerMovement.StartMove();
            }
        }
        else if (!isTargetting)
        {
            playerMovement.StartMove();
            playerAttack.SetEnable(false);
        }
    }

    public override void StopGame()
    {
        base.StopGame();
        playerMovement?.StopMove();
    }
}
