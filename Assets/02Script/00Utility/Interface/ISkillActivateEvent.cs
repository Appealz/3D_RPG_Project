using System;
using UnityEngine;

public interface ISkillActivateEvent
{
    event Action OnSkillActivated;
}
