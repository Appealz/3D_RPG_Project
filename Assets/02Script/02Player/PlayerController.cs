using System.Runtime.CompilerServices;
using UnityEngine;

public struct MpChangeEvent
{
    public float CurrentMP;
    public float MaxMp;

    public MpChangeEvent(float currentMP, float maxMp)
    {
        CurrentMP = currentMP;
        MaxMp = maxMp;
    }
}
public class PlayerStatus
{
    public float moveSpeed;
    public float attackRagne = 25f;
    private float maxMp;
    private float curMp;
    public float maxHp;
    public float curHp;
    public float attackDamage;

    public float MaxMp
    {
        get => maxMp;
        set
        {
            if(value < 0)
            {
                Debug.Log("MaxMp는 0 이상이여야 합니다.");
                return;
            }
            maxMp = value;

            if(curMp > maxMp)
            {
                curMp = maxMp;
            }
        }
    }

    public float CurMp
    {
        get => curMp;
        set
        {
            curMp = Mathf.Clamp(value, 0, maxMp);
            EventBus.Publish(new MpChangeEvent(curMp, maxMp));
        }
    }

    public void RecoverMp(float deltaTime)
    {
        CurMp += 5f * deltaTime;
    }
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

        playerState.OnIdleEvent += playerMovement.StopMove;
        playerState.OnMoveEvent += playerMovement.Move;
        playerState.OnChaseEvent += playerMovement.ChaseMove;
        playerState.OnAttackEvent += playerAttack.Attack;
        playerSkillManager.OnChangeState += playerState.ChangeState;
        playerMovement.OnChangeState += playerState.ChangeState;
        playerAttack.OnChangeState += playerState.ChangeState;

        PCInputManager.OnStop += playerState.ChangeState;

        playerSkillManager.InitSkillAnimMap(playerAnims);
    }

    private void OnDisable()
    {
        playerMovement.moveAnims -= playerAnims.MoveAnims;
        playerMovement.runAnims -= playerAnims.RunAnims;

        playerAttack.OnAttackAnims -= playerAnims.AttackAnims;

        playerState.OnMoveEvent -= playerMovement.Move;
        playerState.OnChaseEvent -= playerMovement.ChaseMove;
        playerState.OnAttackEvent -= playerAttack.Attack;

        playerSkillManager.OnChangeState -= playerState.ChangeState;
        playerMovement.OnChangeState -= playerState.ChangeState;
        playerAttack.OnChangeState -= playerState.ChangeState;

        //inputHandler.OnSkillButtonInput -= playerSkillManager.UseSkill;

        PCInputManager.OnStop -= playerState.ChangeState;
    }

    public void CurrentInputHandler(IInputHandler curHandler)
    {
        inputHandler = curHandler;
        //inputHandler.OnSkillButtonInput += playerSkillManager.PreparedSkill;
    }

    public override void StartGame()
    {
        base.StartGame();
        playerStatus.moveSpeed = 3f;
        playerStatus.CurMp = playerStatus.MaxMp = 100f;
        playerMovement.InitMove(playerStatus.moveSpeed);
        playerState.InitState();
        playerSkillManager.InitStatus(playerStatus);
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();        
        playerState.UpdateState();
        playerSkillManager.UpdateSKillCoolTIme();
        playerStatus.RecoverMp(Time.deltaTime);
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

        switch (skill.myState)
        {
            case StateType.SkillQ:
                playerState.OnQSkillEvent += skill.Activate;
                Debug.Log("스킬 Active 등록");
                skill.OnStateChange += playerState.ChangeState;
                Debug.Log("ChangeState 등록");
                break;
            case StateType.SkillW:
                playerState.OnWSkillEvent += skill.Activate;
                skill.OnStateChange += playerState.ChangeState;
                break;
            case StateType.SkillE:
                playerState.OnESkillEvent += skill.Activate;
                skill.OnStateChange += playerState.ChangeState;
                break;
            case StateType.SkillR:
                playerState.OnRSkillEvent += skill.Activate;
                skill.OnStateChange += playerState.ChangeState;
                break;
        }
    }


}
