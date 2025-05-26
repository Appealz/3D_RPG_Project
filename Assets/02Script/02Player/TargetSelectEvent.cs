using UnityEngine;

public class TargetSelectEvent
{
    public Transform Target { get; private set; }

    public  TargetSelectEvent(Transform target)
    {
        Target = target;
    }
}

public struct TargetPositionEvent
{
    public Vector3 TargetPos { get; private set; }

    public TargetPositionEvent(Vector3 targetPos)
    {
        TargetPos = targetPos;
    }
}