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
    Vector3 movePos;
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
        playerAttack.OnStartMove += playerMovement.StartMove;
        playerAttack.OnAttackAnims += playerAnims.AttackAnims;
    }

    private void OnDisable()
    {
        playerMovement.moveAnims -= playerAnims.MoveAnims;
        playerMovement.runAnims -= playerAnims.RunAnims;
        playerAttack.OnStopMove -= playerMovement.StopMove;
        playerAttack.OnStartMove -= playerMovement.StartMove;
        playerAttack.OnAttackAnims -= playerAnims.AttackAnims;
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
        playerMovement.Move();

        //inputReturn = inputHandler.GetInputClick();
        AttackState();
        //Moving(inputReturn);
    }
    
    //private void Moving(ClickReturn inputReturn)
    //{
    //    if (inputReturn.pos != Vector3.zero)
    //    {
    //        movePos = inputReturn.pos;
    //        playerMovement.ChangeMoveSpeed(2f);
    //        playerMovement.Move(movePos);
    //        targetTrans = null;
    //        isTargetting = false;            
    //    }

    //    if (inputReturn.targetTrans != null)
    //    {
    //        targetTrans = inputReturn.targetTrans;
    //        isTargetting = true;
    //    }

    //    if (isTargetting && targetTrans != null)
    //    {
    //        playerMovement.Move(targetTrans.position);
    //        playerMovement.ChangeMoveSpeed(5f);
    //    }
    //    if (!isTargetting)
    //    {            
    //        playerMovement.StartMove();
    //        playerMovement.Move(movePos);
    //    }
    //    playerMovement.RunAnims(isTargetting);
    //    playerMovement.WalkAnims();
    //}

    private void AttackState()
    {
        if (isTargetting && (targetTrans.position - transform.position).sqrMagnitude < 50f)
        {
            isAttacking = true;
            playerAttack.SetEnable(isAttacking);
            RotateTowardsTarget(targetTrans);
            playerAttack.TargetSetting(targetTrans);
            playerAttack.AttackEvent();
        }
        else if (isTargetting && (targetTrans.position - transform.position).sqrMagnitude >= 50f)
        {
            isAttacking = false;
            playerAttack.SetEnable(isAttacking);
        }
        else if (!isTargetting)
        {            
            playerMovement.StartMove();            
            playerAttack.SetEnable(false);
        }

    }
    void RotateTowardsTarget(Transform target)
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0f; // Y축 고정 (수평 회전만 원할 때)

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        float rotationSpeed = 180f; // 회전 속도 (조절 가능)

        transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed * Time.deltaTime);
    }

    public override void StopGame()
    {
        base.StopGame();
        playerMovement?.StopMove();
    }
}
