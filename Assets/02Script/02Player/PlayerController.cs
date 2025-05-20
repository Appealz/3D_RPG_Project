using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerStatus
{
    public float moveSpeed;
    public float attackRagne = 25f;
    public float maxMp;
    public float curMp;
    public float maxHp;
    public float curHp;
    public float attackDamage;

}
public class PlayerController : ManagerBase
{
    private PlayerMovement playerMovement;
    private IInputHandler inputHandler;    
    private PlayerAnims playerAnims;
    private PlayerStatus playerStatus = new PlayerStatus();
    private PlayerAttack playerAttack;
    private PlayerState playerState;
    private PlayerSkillManager playerSkillManager;

    //[SerializeField]
    //Transform targetTrans;
    //Vector3 movePos;
    //[SerializeField]
    //bool isTargetting = false;
    //bool isAttacking = false;
    //ClickReturn inputReturn;
    private void Awake()
    {
        TryGetComponent<PlayerMovement>(out playerMovement);        
        TryGetComponent<PlayerAnims>(out playerAnims);
        TryGetComponent<PlayerAttack>(out playerAttack);
        TryGetComponent<PlayerState>(out playerState);
        TryGetComponent<PlayerSkillManager>(out playerSkillManager);
    }

    private void OnEnable()
    {
        playerMovement.moveAnims += playerAnims.MoveAnims;
        playerMovement.runAnims += playerAnims.RunAnims;

        playerAttack.OnAttackAnims += playerAnims.AttackAnims;

        playerState.OnMoveEvent += playerMovement.Move;
        playerState.OnChaseEvent += playerMovement.ChaseMove;
        playerState.OnAttackEvent += playerAttack.Attack;
        playerMovement.OnChangeState += playerState.ChangeState;
        playerAttack.OnChangeState += playerState.ChangeState;        
    }

    private void OnDisable()
    {
        playerMovement.moveAnims -= playerAnims.MoveAnims;
        playerMovement.runAnims -= playerAnims.RunAnims;

        playerAttack.OnAttackAnims -= playerAnims.AttackAnims;

        playerState.OnMoveEvent -= playerMovement.Move;
        playerState.OnChaseEvent -= playerMovement.ChaseMove;
        playerState.OnAttackEvent -= playerAttack.Attack;
        playerMovement.OnChangeState -= playerState.ChangeState;
        playerAttack.OnChangeState -= playerState.ChangeState;

        inputHandler.OnSkillInput -= playerSkillManager.UseSkill;
    }

    public void CurrentInputHandler(IInputHandler curHandler)
    {
        inputHandler = curHandler;
        inputHandler.OnSkillInput += playerSkillManager.UseSkill;
    }

    public override void StartGame()
    {
        base.StartGame();
        playerStatus.moveSpeed = 2f;
        playerMovement.InitMove(playerStatus.moveSpeed);
        playerState.InitState();
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();        
        playerState.UpdateState();    
    }

    public override void StopGame()
    {
        base.StopGame();
        playerMovement?.StopMove();
    }

    public void RegistSkill(KeyCode key, ISkill skill)
    {
        playerSkillManager.AddSkill(key, skill);
        inputHandler.BindKeyToSkill(key, skill.myType);
    }


}
