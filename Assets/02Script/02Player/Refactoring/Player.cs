using UnityEngine;
using UnityEngine.AI;

public class PlayerStatus_Fixed
{
    public PlayerStatus_Fixed(PlayerData playerData)
    {
        moveSpeed = playerData.moveSpeed;
        maxHp = playerData.maxHp;
        maxMp = playerData.maxMp;
        attackDamage = playerData.attackDamage;
        attackRagne = playerData.attackRagne;
    }

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



    public float MaxMp
    {
        get => maxMp;
        set
        {
            if (value < 0)
            {
                Debug.Log("MaxMp는 0 이상이여야 합니다.");
                return;
            }
            maxMp = value;

            if (curMp > maxMp)
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

public class Player : ManagerBase
{
    [SerializeField]
    private PlayerData playerData; 

    private NavMeshAgent agent;
    public NavMeshAgent Agent => agent;
    private PlayerAnims anims;
    public PlayerAnims Anims => anims;


    private PlayerFSM playerFSM;
    public Vector3 targetPos { get; private set; }

    private PlayerStatus_Fixed playerStatus;
    public PlayerStatus_Fixed PlayerStatus => playerStatus;

    private void Awake()
    {
        if(!TryGetComponent<NavMeshAgent>(out agent))
        {
            Debug.Log("agent is not ref");
        }
        playerFSM = new PlayerFSM(this);
        playerStatus = new PlayerStatus_Fixed(playerData);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<TargetPositionEvent>(SetTargetPos);
        Damage_Event.OnDamageChange += Handle_OnDamaged;
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<TargetPositionEvent>(SetTargetPos);
        Damage_Event.OnDamageChange -= Handle_OnDamaged;
    }

    public override void StartGame()
    {
        base.StartGame();
        playerFSM.Init();
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        playerFSM.StateUpdate();
    }

    public void SetTargetPos(TargetPositionEvent targetPositionEvent)
    {
        targetPos = targetPositionEvent.TargetPos;
    }

    public void Handle_OnDamaged(DamageInfo damageInfo)
    {
        if(damageInfo.defender == gameObject)
        {
            playerStatus.CurHP -= damageInfo.damage;
        }
    }


    //private PlayerMovement playerMovement;
    //private IInputHandler inputHandler;
    //private PlayerAnims playerAnims;
    //private PlayerAttack playerAttack;
    //private PlayerStatus playerStatus;
    //private PlayerState playerState;
    //private PlayerSkillManager playerSkillManager;
    //private PlayerAnimManager playerAnimManager;
    //private PlayerHitbox playerHitbox;

    //private Action qSkillHandler;
    //private Action wSkillHandler;
    //private Action eSkillHandler;
    //private Action rSkillHandler;

    //private void Awake()
    //{
    //    TryGetComponent<PlayerMovement>(out playerMovement);
    //    TryGetComponent<PlayerAnims>(out playerAnims);
    //    TryGetComponent<PlayerAttack>(out playerAttack);
    //    TryGetComponent<PlayerState>(out playerState);
    //    TryGetComponent<PlayerSkillManager>(out playerSkillManager);
    //    TryGetComponent<PlayerAnimManager>(out playerAnimManager);
    //    TryGetComponent<PlayerHitbox>(out playerHitbox);
    //    playerStatus = new PlayerStatus(gameObject);
    //}



    //private void OnEnable()
    //{
    //    playerMovement.moveAnims += playerAnims.MoveAnims;
    //    playerMovement.runAnims += playerAnims.RunAnims;

    //    playerAttack.OnAttackAnims += playerAnims.AttackAnims;

    //    playerState.OnIdleEvent += playerMovement.StopMove;
    //    playerState.OnMoveEvent += playerMovement.Move;
    //    playerState.OnChaseEvent += playerMovement.ChaseMove;
    //    playerState.OnAttackEvent += playerAttack.Attack;
    //    playerSkillManager.OnChangeState += playerState.ChangeState;
    //    playerMovement.OnChangeState += playerState.ChangeState;
    //    playerAttack.OnChangeState += playerState.ChangeState;

    //    PCInputManager.OnStop += playerState.ChangeState;

    //    playerSkillManager.InitSkillAnimMap(playerAnims);
    //}

    //private void OnDisable()
    //{
    //    playerMovement.moveAnims -= playerAnims.MoveAnims;
    //    playerMovement.runAnims -= playerAnims.RunAnims;

    //    playerAttack.OnAttackAnims -= playerAnims.AttackAnims;

    //    playerState.OnMoveEvent -= playerMovement.Move;
    //    playerState.OnChaseEvent -= playerMovement.ChaseMove;
    //    playerState.OnAttackEvent -= playerAttack.Attack;

    //    playerSkillManager.OnChangeState -= playerState.ChangeState;
    //    playerMovement.OnChangeState -= playerState.ChangeState;
    //    playerAttack.OnChangeState -= playerState.ChangeState;

    //    //inputHandler.OnSkillButtonInput -= playerSkillManager.UseSkill;

    //    PCInputManager.OnStop -= playerState.ChangeState;
    //}

    //public void CurrentInputHandler(IInputHandler curHandler)
    //{
    //    inputHandler = curHandler;
    //    //inputHandler.OnSkillButtonInput += playerSkillManager.PreparedSkill;
    //}

    //public override void StartGame()
    //{
    //    base.StartGame();
    //    playerStatus.moveSpeed = 4f;
    //    playerStatus.CurMp = playerStatus.MaxMp = 100f;
    //    playerStatus.CurHP = playerStatus.MaxHp = 100f;
    //    playerMovement.InitMove(playerStatus.moveSpeed);
    //    playerState.InitState();
    //    playerSkillManager.InitStatus(playerStatus);
    //    playerHitbox.InitStatus(playerStatus);
    //}

    //public override void CustomUpdate()
    //{
    //    base.CustomUpdate();
    //    playerState.UpdateState();
    //    playerSkillManager.UpdateSKillCoolTIme();
    //    playerStatus.RecoverMp(Time.deltaTime);
    //    playerStatus.RecoverHp(Time.deltaTime);

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        ActionQueue.Instance.QueueCheck();
    //    }
    //}

    //public override void StopGame()
    //{
    //    base.StopGame();
    //    playerMovement?.StopMove();
    //    playerSkillManager?.ReleaseAllSkills();
    //}

    //public void RegistSkill(KeyCode key, ISkill skill)
    //{
    //    playerSkillManager.AddSkill(key, skill);
    //    inputHandler.BindKeyToSkill(key, skill.myType);


    //    switch (skill.myState)
    //    {
    //        case StateType.SkillQ:
    //            playerState.OnQSkillEvent += skill.Activate;
    //            skill.OnStateChange += playerState.ChangeState;
    //            qSkillHandler = () => playerAnimManager.PlayAnimation("Qskill", skill);
    //            skill.OnSkillActivated += qSkillHandler;
    //            break;
    //        case StateType.SkillW:
    //            playerState.OnWSkillEvent += skill.Activate;
    //            skill.OnStateChange += playerState.ChangeState;
    //            wSkillHandler = () => playerAnimManager.PlayAnimation("Wskill", skill);
    //            skill.OnSkillActivated += wSkillHandler;
    //            break;
    //        case StateType.SkillE:
    //            playerState.OnESkillEvent += skill.Activate;
    //            skill.OnStateChange += playerState.ChangeState;
    //            eSkillHandler = () => playerAnimManager.PlayAnimation("Eskill", skill);
    //            skill.OnSkillActivated += eSkillHandler;
    //            break;
    //        case StateType.SkillR:
    //            playerState.OnRSkillEvent += skill.Activate;
    //            skill.OnStateChange += playerState.ChangeState;
    //            rSkillHandler = () => playerAnimManager.PlayAnimation("Rskill", skill);
    //            skill.OnSkillActivated += rSkillHandler;
    //            break;
    //    }
    //}

    //public void ReleaseSkill(ISkill skill)
    //{
    //    switch (skill.myState)
    //    {
    //        case StateType.SkillQ:
    //            playerState.OnQSkillEvent -= skill.Activate;
    //            skill.OnStateChange -= playerState.ChangeState;
    //            skill.OnSkillActivated -= qSkillHandler;
    //            break;
    //        case StateType.SkillW:
    //            playerState.OnWSkillEvent -= skill.Activate;
    //            skill.OnStateChange -= playerState.ChangeState;
    //            skill.OnSkillActivated -= wSkillHandler;
    //            break;
    //        case StateType.SkillE:
    //            playerState.OnESkillEvent -= skill.Activate;
    //            skill.OnStateChange -= playerState.ChangeState;
    //            skill.OnSkillActivated -= eSkillHandler;
    //            break;
    //        case StateType.SkillR:
    //            playerState.OnRSkillEvent -= skill.Activate;
    //            skill.OnStateChange -= playerState.ChangeState;
    //            skill.OnSkillActivated -= rSkillHandler;
    //            break;
    //    }
    //}

}
