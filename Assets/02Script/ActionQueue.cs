using System.Collections.Generic;
using UnityEngine;

public class ActionQueue : Singleton<ActionQueue>
{
    Queue<StateType> stateQueue = new Queue<StateType>();
    StateType newState;
    StateType currentState;
    public bool isEmpty => stateQueue.Count == 0;
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
    public StateType PeekNext()
    {
        if (stateQueue.Count == 0)
        {
            return StateType.Idle;
        }

        return stateQueue.Peek();
    }
}
