using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimEventManager : MonoBehaviour
{
    [SerializeField]
    public List<AnimationEventData> allEventData;

    Dictionary<string, AnimationEventData> events;

    private void Awake()
    {
        //events = allEventData.ToDictionary(x => x.animationName);
        events = new Dictionary<string, AnimationEventData>();
        foreach (var item in allEventData)
        {
            events[item.animationName] = item;
        }
    }
}
