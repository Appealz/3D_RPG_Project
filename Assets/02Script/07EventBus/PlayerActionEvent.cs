using UnityEngine;

public class PlayerMoveLockEvent
{
    public bool CanMove;
    public PlayerMoveLockEvent(bool canMove)
    {
        CanMove = canMove;
    }
}

public class RotateToTargetEvent
{
    public Transform Target { get; }

    public RotateToTargetEvent(Transform target)
    {
        Target = target;
    }
}

public struct RotateToPosEvent
{
    public Vector3 Position;

    public RotateToPosEvent(Vector3 position)
    {
        Position = position;
    }
}