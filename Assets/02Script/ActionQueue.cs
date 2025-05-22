using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : Singleton<ActionQueue>
{
    Queue<StateType> stateQueue = new Queue<StateType>();
    StateType newState;
    public void EnqueueAction(StateType curState)
    {
        Debug.Log($"ť ���� :{curState}");
        if (curState == StateType.Attack || curState == StateType.SkillQ || curState == StateType.SkillR)
        {
            stateQueue.Enqueue(StateType.Chase);
        }
        stateQueue.Enqueue(curState);
    }

    public StateType DequeueAction()
    {
        if(HasQueue())
        {
            newState = stateQueue.Dequeue();
            Debug.Log($"ť ���� : {newState}");
        }
        return newState;
    }

    public void ClearQueue()
    {
        stateQueue.Clear();
    }

    public bool HasQueue()
    {
        return stateQueue.Count > 0;
    }


}
