using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM
{    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public StateBase currentState;

    public Player player;
    
    private Dictionary<StateType, StateBase> stateDictionary = new Dictionary<StateType, StateBase>();

    public PlayerFSM(Player newPlayer)
    {
        player = newPlayer;
        stateDictionary[StateType.Idle] = new IdleState(player, this);
        stateDictionary[StateType.Attack] = new AttackState(player, this, player.AttackHandle);
        stateDictionary[StateType.Chase] = new ChaseState(player, this, player.Anims, player.Movement);
        stateDictionary[StateType.Move] = new MoveState(player, this, player.Movement);
        //stateDictionary[StateType.SkillQ] = new QSkillState(player, this, player.SkillManager.GetSkill(SkillType.Q_Skill));
    }

    public void Init()
    {
        currentState = stateDictionary[StateType.Idle];
        currentState.StateEnter();
    }



    public void ChangeState(StateType newStateType)
    {
        if (currentState == stateDictionary[newStateType])
            return; // 상태 전환 필요 없음

        if (stateDictionary.TryGetValue(newStateType, out StateBase newState))
        {
            if (currentState != null)
            {
                currentState.StateExit();
            }            
            currentState = newState;
            Debug.Log($"{currentState}로 상태 변경");
            if (currentState != null)
            {
                currentState.StateEnter();
            }
        }
        else
        {
            Debug.Log($"{newStateType} is not registered stateType");
        }
    }

    public void StateUpdate()
    {        
        currentState?.StateUpdate();

        if (!ActionQueue.Instance.isEmpty)
        {
            if(currentState.isDone)
            {
                StateType nextState = ActionQueue.Instance.DequeueAction();
                ChangeState(nextState);
            }            
        }
    }
}
