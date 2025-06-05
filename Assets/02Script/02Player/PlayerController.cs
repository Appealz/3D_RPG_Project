using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public struct MpChangeEvent
{
    public float CurrentMP;
    public float MaxMp;
    public GameObject Publisher;

    public MpChangeEvent(GameObject publisher, float currentMP, float maxMp)
    {
        Publisher = publisher;
        CurrentMP = currentMP;
        MaxMp = maxMp;
    }
}

public struct HpChangeEvent
{
    public float CurrentHp;
    public float MaxHp;
    public GameObject Publisher;

    public HpChangeEvent(GameObject publisher, float currentHp, float maxHp)
    {
        Publisher = publisher;
        CurrentHp = currentHp;
        MaxHp = maxHp;
    }
}

public class PlayerStatus
{
    public float moveSpeed;
    public float attackRagne = 25f;
    private float maxMp;
    private float curMp;
    private float maxHp;

    public float MaxHp
    {
        get => maxMp;
        set
        {
            if (value < 0)
            {
                Debug.Log("MaxHp는 0 이상이여야 합니다.");
                return;
            }
            maxHp = value;

            if (curHp > maxHp)
            {
                curHp = maxHp;
            }
        }
    }
    private float curHp;

    public float CurHP
    {
        get => curHp;
        set
        {
            curHp = Mathf.Clamp(value, 0, maxHp);
            EventBus.Publish(new HpChangeEvent(Player, curHp, maxHp));
        }
    }
    public float attackDamage;
    public GameObject Player;

    public PlayerStatus(GameObject player)
    {
        Player = player;
    }

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
            EventBus.Publish(new MpChangeEvent(Player, curMp, maxMp));
        }
    }

    public void RecoverMp(float deltaTime)
    {
        CurMp += 5f * deltaTime;
    }

    public void RecoverHp(float deltaTime)
    {
        CurHP += 1f * deltaTime;
    }
}


public class PlayerController : ManagerBase
{
    private PlayerMovement playerMovement;
    private IInputHandler inputHandler;
    private PlayerAnims playerAnims;
    private PlayerAttack playerAttack;
    private PlayerStatus playerStatus;
    private PlayerState playerState;
    private PlayerSkillManager playerSkillManager;
    private PlayerAnimManager playerAnimManager;
    private PlayerHitbox playerHitbox;

    private Action qSkillHandler;
    private Action wSkillHandler;
    private Action eSkillHandler;
    private Action rSkillHandler;

    private void Awake()
    {
        TryGetComponent<PlayerMovement>(out playerMovement);
        TryGetComponent<PlayerAnims>(out playerAnims);
        TryGetComponent<PlayerAttack>(out playerAttack);
        TryGetComponent<PlayerState>(out playerState);
        TryGetComponent<PlayerSkillManager>(out playerSkillManager);
        TryGetComponent<PlayerAnimManager>(out playerAnimManager);
        TryGetComponent<PlayerHitbox>(out playerHitbox);
        playerStatus = new PlayerStatus(gameObject);
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
        playerStatus.moveSpeed = 4f;
        playerStatus.CurMp = playerStatus.MaxMp = 100f;
        playerStatus.CurHP = playerStatus.MaxHp = 100f;
        playerMovement.InitMove(playerStatus.moveSpeed);
        playerState.InitState();
        playerSkillManager.InitStatus(playerStatus);
        playerHitbox.InitStatus(playerStatus);
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();        
        playerState.UpdateState();
        playerSkillManager.UpdateSKillCoolTIme();
        playerStatus.RecoverMp(Time.deltaTime);
        playerStatus.RecoverHp(Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            ActionQueue.Instance.QueueCheck();
        }        
    }

    public override void StopGame()
    {
        base.StopGame();
        playerMovement?.StopMove();
        playerSkillManager?.ReleaseAllSkills();        
    }

    public void RegistSkill(KeyCode key, ISkill skill)
    {
        playerSkillManager.AddSkill(key, skill);
        inputHandler.BindKeyToSkill(key, skill.myType);
        

        switch (skill.myState)
        {
            case StateType.SkillQ:
                playerState.OnQSkillEvent += skill.Activate;                
                skill.OnStateChange += playerState.ChangeState;
                qSkillHandler = () => playerAnimManager.PlayAnimation("Qskill", skill);
                skill.OnSkillActivated += qSkillHandler;
                break;
            case StateType.SkillW:
                playerState.OnWSkillEvent += skill.Activate;
                skill.OnStateChange += playerState.ChangeState;
                wSkillHandler = () => playerAnimManager.PlayAnimation("Wskill", skill);
                skill.OnSkillActivated += wSkillHandler;
                break;
            case StateType.SkillE:
                playerState.OnESkillEvent += skill.Activate;
                skill.OnStateChange += playerState.ChangeState;
                eSkillHandler = () => playerAnimManager.PlayAnimation("Eskill", skill);
                skill.OnSkillActivated += eSkillHandler;
                break;
            case StateType.SkillR:
                playerState.OnRSkillEvent += skill.Activate;
                skill.OnStateChange += playerState.ChangeState;
                rSkillHandler = () => playerAnimManager.PlayAnimation("Rskill", skill);
                skill.OnSkillActivated += rSkillHandler;
                break;
        }
    }

    public void ReleaseSkill(ISkill skill)
    {
        switch (skill.myState)
        {
            case StateType.SkillQ:
                playerState.OnQSkillEvent -= skill.Activate;                
                skill.OnStateChange -= playerState.ChangeState;
                skill.OnSkillActivated -= qSkillHandler;
                break;
            case StateType.SkillW:
                playerState.OnWSkillEvent -= skill.Activate;    
                skill.OnStateChange -= playerState.ChangeState;
                skill.OnSkillActivated -= wSkillHandler;
                break;
            case StateType.SkillE:
                playerState.OnESkillEvent -= skill.Activate;
                skill.OnStateChange -= playerState.ChangeState;
                skill.OnSkillActivated -= eSkillHandler;
                break;
            case StateType.SkillR:
                playerState.OnRSkillEvent -= skill.Activate;
                skill.OnStateChange -= playerState.ChangeState;
                skill.OnSkillActivated -= rSkillHandler;
                break;
        }
    }


}
