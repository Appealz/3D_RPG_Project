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
        if(inputHandler.TryGetRightClickPosition(out Vector3 targetPos))
        {
            player.SetTargetPos(targetPos);
            player.ReadyToAttack(false);
            player.preparedSkillType = null;
            player.ChangeState(StateType.Move);
        }
        else if(inputHandler.TryGetRightClickTarget(out Transform targetTrans))
        {
            player.SetTargetTrans(targetTrans);
            player.ChangeState(StateType.Attack);
        }

        // A클릭
        if(inputHandler.IsAttackKeyDown())
        {
            player.ReadyToAttack(true);
        }

        // A클릭 된 상태
        if(player.IsAttackReady)
        {
            if (inputHandler.TryGetAttackTargetClick(out Transform target))
            {
                player.SetTargetTrans(target);
                player.ChangeState(StateType.Move);
            }
            else if (inputHandler.TryGetAttackGroundClick(out Vector3 movePos))
            {
                player.SetTargetPos(movePos);
                player.ReadyToAttack(false);
                player.preparedSkillType = null;
                player.ChangeState(StateType.Move);
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
            player.ChangeState(StateType.Idle);
        }

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
