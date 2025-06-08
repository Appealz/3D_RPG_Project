using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimManager : MonoBehaviour
{
    [SerializeField]
    private AnimEventManager animEventManager;

    private Animator anim;

    private List<TimeEvent> currentEvent;
    private int currentEventIndex;
    private float animElapsedTime;

    private bool animRunning;

    private ISkill currentSkill;
    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    public void PlayAnimation(string name, ISkill iskill)
    {
        currentEvent = animEventManager.GetEventsForAnimation(name);
        Debug.Log($"Count : {currentEvent.Count} ");
        animElapsedTime = 0f;
        currentEventIndex = 0;
        animRunning = true;
        currentSkill = iskill;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!animRunning || currentEvent == null) return;

        animElapsedTime += Time.deltaTime;

        while (currentEventIndex < currentEvent.Count && currentEvent[currentEventIndex].time <= animElapsedTime)
        {
            ExecuteTimedEvent(currentEvent[currentEventIndex]);
            currentEventIndex++;
        }
    }

    private void ExecuteTimedEvent(TimeEvent evt)
    {
        Debug.Log($"[타이밍 이벤트] {evt.eventType} at {evt.time}s | param: {evt.param}");

        switch (evt.eventType)
        {
            case "Fire":
                // 예: Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                currentSkill.CreateEffect();
                break;
            case "Cancel":
                currentSkill.CancelAble();                
                break;
            case "End":
                currentSkill.Finish();
                animRunning = false;
                break;
        }
    }
}
