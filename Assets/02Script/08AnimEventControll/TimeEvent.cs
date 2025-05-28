using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeEvent
{
    public float time;
    public string eventType;
    public string param;
}

[System.Serializable]
public class AnimationEventData
{
    public string animationName;
    public List<TimeEvent> animEventTimer;    
}