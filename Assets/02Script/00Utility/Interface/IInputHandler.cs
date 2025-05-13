using System;
using UnityEngine;

public interface IInputHandler
{
    ClickReturn GetInputClick();
}

public struct ClickReturn
{
    public Vector3 pos;
    public Transform targetTrans;
}

