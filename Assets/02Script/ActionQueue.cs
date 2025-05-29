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
        Debug.Log($"{curState} 상태 큐 진입");
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

    public StateType PeekNext()
    {
        if (stateQueue.Count == 0)
        {
            //Debug.LogWarning("ActionQueue가 비어있습니다. 기본 상태를 반환합니다.");
            return StateType.Idle;
        }

        return stateQueue.Peek();
    }
    public void ClearQueue()
    {
        stateQueue.Clear();
    }

    public bool HasQueue()
    {
        return stateQueue.Count > 0;
    }

    public void QueueCheck()
    {
        if (stateQueue.Count == 0)
        {
            Debug.Log("ActionQueue는 비어있습니다.");
            return;
        }

        Debug.Log("=== 현재 ActionQueue 상태 ===");
        foreach (var action in stateQueue)
        {
            Debug.Log($"Queued State: {action}");
        }
    }

}
