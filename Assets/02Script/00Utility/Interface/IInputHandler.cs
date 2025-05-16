using System;
using UnityEngine;

public interface IInputHandler
{
    void GetInputClick();
    void CustomUpdate();
}

public struct ClickReturn
{
    public Vector3 pos;
    public Transform targetTrans;
}

