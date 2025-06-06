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
        stateDictionary[StateType.Attack] = new AttackState(player, this);
        stateDictionary[StateType.Chase] = new ChaseState(player, this);
        stateDictionary[StateType.Move] = new MoveState(player, this);        
    }

    public void Init()
    {
        currentState = stateDictionary[StateType.Idle];
        currentState.StateEnter();
    }



    public void ChangeState(StateType newStateType)
    {
        if (stateDictionary.TryGetValue(newStateType, out StateBase newState))
        {
            if (currentState != null)
            {
                currentState.StateExit();
            }
            currentState = newState;
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
