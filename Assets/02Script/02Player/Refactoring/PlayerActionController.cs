using UnityEngine;

public class PlayerActionController : ManagerBase
{
    private IInputHandle inputHandler;
    private Player player;

    public void BindToInputHandler(IInputHandle newInputHandler, Player newPlayer)
    {
        inputHandler = newInputHandler;
        player = newPlayer;
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();

        // 마우스 우클릭
        if(inputHandler.TryGetRightClickTarget(out Transform targetTrans))
        {
            Debug.Log($"마우스 우클릭 타겟 설정{targetTrans}");
            player.SetTargetTrans(targetTrans);
            player.ChangeState(StateType.Attack);            
        }
        else if (inputHandler.TryGetRightClickPosition(out Vector3 targetPos))
        {
            player.SetTargetPos(targetPos);
            player.ReadyToAttack(false);
            player.preparedSkillType = null;            

            bool allowForceMove = ActionQueue.Instance.HasQueue();
            player.ChangeState(StateType.Move, force: allowForceMove);
            ActionQueue.Instance.ClearQueue();
        }

        // A클릭
        if (inputHandler.IsAttackKeyDown())
        {
            player.ReadyToAttack(true);
        }

        // A클릭 된 상태
        if(player.IsAttackReady)
        {
            if (inputHandler.TryGetAttackTargetClick(out Transform target))
            {
                Debug.Log($"A클릭 상태 마우스 우클릭 타겟 설정{target}");
                player.SetTargetTrans(target);
                player.ChangeState(StateType.Attack);            
            }
            else if (inputHandler.TryGetAttackGroundClick(out Vector3 movePos))
            {
                player.SetTargetPos(movePos);
                player.ReadyToAttack(false);
                player.preparedSkillType = null;                

                bool allowForceMove = ActionQueue.Instance.HasQueue();
                player.ChangeState(StateType.Move, force: allowForceMove);
                ActionQueue.Instance.ClearQueue();
            }
        }

        // esc 클릭
        if(inputHandler.IsCancelInput())
        {
            player.ReadyToAttack(false);
            player.preparedSkillType = null;
        }
        
        // s 클릭
        if(inputHandler.IsStopRequested())
        {
            player.ReadyToAttack(false);
            player.preparedSkillType = null;
            player.ChangeState(StateType.Idle, force: true);
        }

        // 스킬 키 입력
        if(inputHandler.TryGetSkillKeyInput(out SkillType newSkill))
        {
            player.preparedSkillType = newSkill;
        }
            
        if(player.preparedSkillType.HasValue)
        {
            if(inputHandler.TryGetSkillTarget(out Transform skillTarget))
            {
                player.SetTargetTrans(skillTarget);
            }
            else if(inputHandler.TryGetSkillPosition(out Vector3 skillPosition))
            {                
                player.SetTargetPos(skillPosition);
            }
            else if(inputHandler.TryGetSkillDirection(out Vector3 skillDir))
            {
                player.SetTargetPos(skillDir);
            }
        }
    }
}
