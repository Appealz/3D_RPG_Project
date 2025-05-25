using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : Singleton<ActionQueue>
{
    Queue<StateType> stateQueue = new Queue<StateType>();
    StateType newState;
    public void EnqueueAction(StateType curState)
    {
        stateQueue.Enqueue(curState);
    }

    public StateType DequeueAction()
    {
        if(HasQueue())
        {
            newState = stateQueue.Dequeue();
            Debug.Log($"현재 큐 개수 : {stateQueue.Count}");
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
