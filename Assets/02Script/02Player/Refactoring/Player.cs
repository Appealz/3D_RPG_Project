using UnityEngine;
using UnityEngine.AI;

public class PlayerStatus_Fixed
{
    public PlayerStatus_Fixed(PlayerData playerData)
    {
        moveSpeed = playerData.moveSpeed;
        MaxHp = playerData.maxHp;
        MaxMp = playerData.maxMp;
        attackDamage = playerData.attackDamage;
        attackRagne = playerData.attackRagne;

        CurHp = MaxHp;
        CurMp = MaxMp;  
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

    public float CurHp
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
        CurHp += 1f * deltaTime;
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
    public Transform targetTrans { get; private set; }
    private PlayerStatus_Fixed playerStatus;
    public PlayerStatus_Fixed PlayerStatus => playerStatus;

    private PlayerMove movement;
    public PlayerMove Movement => movement;
    private PlayerAttackHandle attackHandle;
    public PlayerAttackHandle AttackHandle => attackHandle;

    private SkillManager skillManager;
    public SkillManager playerSkillManager => skillManager;

    private bool isAttackReady;
    public bool IsAttackReady => isAttackReady;

    public SkillType? preparedSkillType;
    public bool isSkillPrepared;

    private PlayerAnimManager playerAnimManager;

    private void Awake()
    {
        if(!TryGetComponent<NavMeshAgent>(out agent))
        {
            Debug.Log("agent is not ref");
        }

        if(!TryGetComponent<PlayerAnims>(out anims))
        {
            Debug.Log("anims is not ref");
        }

        if(!TryGetComponent<PlayerAttackHandle>(out attackHandle))
        {
            Debug.Log("attackHandle is not ref");
        }
        if(!TryGetComponent<PlayerMove>(out movement))
        {
            Debug.Log("movement is not ref");
        }
        if(!TryGetComponent<SkillManager>(out skillManager))
        {
            Debug.Log("skillManager is not ref");
        }
        if(!TryGetComponent<PlayerAnimManager>(out playerAnimManager))
        {
            Debug.Log("playerAnimManager is not ref");
        }

        playerStatus = new PlayerStatus_Fixed(playerData);

        movement.InitMove(playerStatus.moveSpeed, anims);
        attackHandle.InitAttack(playerStatus, anims);
        skillManager.InitStatus(playerStatus);
    }

    private void OnEnable()
    {
        Damage_Event.OnDamageChange += Handle_OnDamaged;
    }

    private void OnDisable()
    {
        Damage_Event.OnDamageChange -= Handle_OnDamaged;
    }

    public override void StartGame()
    {
        base.StartGame();
        playerFSM = new PlayerFSM(this);
        playerFSM.Init();
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if (playerFSM == null)
        {
            Debug.LogWarning("playerFSM is null in CustomUpdate.");
            return;
        }

        // 상태 업데이트용
        playerFSM.StateUpdate();
        // 스킬 쿨타임갱신용
        skillManager.UpdateSKillCoolTIme();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActionQueue.Instance.QueueCheck();
        }
    }

    public void SetTargetPos(Vector3 newTargetPosition)
    {
        targetPos = newTargetPosition;
    }

    public void SetTargetTrans(Transform newTargetTrans)
    {
        targetTrans = newTargetTrans;
    }

    public void ReadyToAttack(bool newIsOn)
    {
        isAttackReady = newIsOn;
        if(IsAttackReady)
        {
            EventBus.Publish(new indicatorEvent(IndicatorType.Circle, Vector3.zero, Mathf.Sqrt(playerStatus.attackRagne)));
        }        
    }

    public void ChangeState(StateType newStateType, bool force = false)
    {
        playerFSM.ChangeState(newStateType, force);
    }

    public void Handle_OnDamaged(DamageInfo damageInfo)
    {
        if(damageInfo.defender == gameObject)
        {
            playerStatus.CurHp -= damageInfo.damage;
        }
    }

    public void PrepareToSkill(SkillType newType)
    {
        //Debug.Log($"{gameObject.name}에서 {newType} 준비 후 skillManager 전달");
        isSkillPrepared = true;
        skillManager.PrepareSkill(newType);
    }

    public void UsePreparedSkill()
    {
        StateType changeState;
        changeState = skillManager.UseSkill();
        playerFSM.ChangeState(changeState, force: true);
        isSkillPrepared = false;
    }

    public void RegistSkill(ISkill newSKill)
    {
        skillManager.AddSkill(newSKill);
        newSKill.OnSkillActivated += () => playerAnimManager.PlayAnimation(newSKill.skillName, newSKill);

        switch(newSKill.myType)
        {
            case SkillType.Q_Skill:
                newSKill.OnSkillActivated += () => anims.QSkillAnims();
                break;
            case SkillType.W_Skill:
                newSKill.OnSkillActivated += () => anims.WSkillAnims();
                break;
                case SkillType.E_Skill:
                newSKill.OnSkillActivated += () => anims.ESkillAnims();
                break;
            case SkillType.R_Skill:
                newSKill.OnSkillActivated += () => anims.RSkillAnims();
                break;
        }
    }
  

}
