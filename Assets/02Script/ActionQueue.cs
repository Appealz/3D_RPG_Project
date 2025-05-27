using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : Singleton<ActionQueue>
{
    Queue<StateType> stateQueue = new Queue<StateType>();
    StateType newState;
    //StateType currentState = StateType.Idle;
    public void EnqueueAction(StateType curState)
    {
        //if(currentState != curState)
        //{
        //    currentState = curState;
            stateQueue.Enqueue(curState);            
        //}
    }

    public StateType DequeueAction()
    {
        if(HasQueue())
        {
            newState = stateQueue.Dequeue();
            Debug.Log($"현재 큐 개수 : {stateQueue.Count}");
            return newState;
        }
        Debug.Log("큐 비어있음");
        return StateType.Idle;
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
