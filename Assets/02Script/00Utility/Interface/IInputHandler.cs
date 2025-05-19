using System;
using UnityEngine;

public interface IInputHandler
{
    event Action<SkillType> OnSkillInput;
    void GetInputClick();
    void CustomUpdate();
    void BindKeyToSkill(KeyCode key, SkillType skillType);
}

public struct ClickReturn
{
    public Vector3 pos;
    public Transform targetTrans;
}

