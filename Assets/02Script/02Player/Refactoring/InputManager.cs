using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public class InputManager : ManagerBase, IInputHandle
{
    // CursorManager    
    public static event Action<StateType> OnStop;

    // SkillManager
    private Dictionary<KeyCode, SkillType> keySkillBindings = new Dictionary<KeyCode, SkillType>();

    private bool isAttackOn;

    private bool isReadySkill = false;

    private SkillType? currentReadySkill = null;
    private void OnEnable()
    {
        //EventBus.Subscribe<SkillPreparedEvent>(OnSkillPrepared);
    }

    private void OnDisable()
    {
        //EventBus.UnSubscribe<SkillPreparedEvent>(OnSkillPrepared);
    }


    private void CreateMouseIndicator(RaycastHit hit)
    {
        GameObject obj = ObjectPoolManager.Instance.pool[1].PopObj();
        Vector3 setPoint = new Vector3(hit.point.x, hit.point.y + 0.2f, hit.point.z);
        obj.transform.position = setPoint;
    }
    public void BindKeyToSkill(KeyCode key, SkillType skillType)
    {
        keySkillBindings[key] = skillType;
    }

    public bool TryGetRightClickPosition(out Vector3 position)
    {
        position = Vector3.zero;
        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                CreateMouseIndicator(hit);
                EventBus.Publish(new CursorEventData(cursorType.Idle));
                position = hit.point;
                EventBus.Publish(new HideIndicatorEvent());
                return true;
            }
        }        
        return false;
    }

    public bool TryGetRightClickTarget(out Transform target)
    {
        target = null;
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {
                CreateMouseIndicator(hit);
                EventBus.Publish(new CursorEventData(cursorType.Idle));
                EventBus.Publish(new HideIndicatorEvent());
                target = hit.transform;
                return true;
            }
        }
        return false;
    }

    public bool IsAttackKeyDown()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventBus.Publish(new CursorEventData(cursorType.Aim));
            EventBus.Publish(new indicatorEvent(IndicatorType.Circle, Vector3.zero, 5f));
            return true;
        }
        return false;
    }

    public bool TryGetAttackTargetClick(out Transform target)
    {
        target = null;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {
                CreateMouseIndicator(hit);
                EventBus.Publish(new CursorEventData(cursorType.Idle));
                EventBus.Publish(new HideIndicatorEvent());
                target = hit.transform;
                return true;
            }
        }
        return false;
    }

    public bool TryGetAttackGroundClick(out Vector3 groundPos)
    {
        groundPos = Vector3.zero;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                CreateMouseIndicator(hit);
                EventBus.Publish(new CursorEventData(cursorType.Idle));
                EventBus.Publish(new HideIndicatorEvent());
                groundPos = hit.point;
                return true;
            }
        }
        return false;
    }

    public bool TryGetSkillKeyInput(out SkillType skillType)
    {
        skillType = SkillType.None;
        foreach (var binding in keySkillBindings)
        {
            if (Input.GetKeyDown(binding.Key))
            {
                skillType = binding.Value;
                return true;
            }
        }
        return false;
    }

    public bool IsSkillReady(out SkillType skillType)
    {
        skillType = SkillType.None;
        return false;
    }

    public bool TryGetSkillTarget(out Transform target)
    {
        target = null;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {
                CreateMouseIndicator(hit);
                EventBus.Publish(new CursorEventData(cursorType.Idle));
                EventBus.Publish(new HideIndicatorEvent());
                target = hit.transform;
                return true;
            }
        }
        return false;
    }

    public bool TryGetSkillDirection(out Vector3 direction)
    {
        direction = Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                CreateMouseIndicator(hit);
                EventBus.Publish(new CursorEventData(cursorType.Idle));
                EventBus.Publish(new HideIndicatorEvent());
                direction = hit.point;
                return true;
            }
        }
        return false;
    }

    public bool TryGetSkillPosition(out Vector3 position)
    {
        position = Vector3.zero;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                CreateMouseIndicator(hit);
                EventBus.Publish(new CursorEventData(cursorType.Idle));
                EventBus.Publish(new HideIndicatorEvent());
                position = hit.point;
                return true;
            }
        }
        return false;
    }

    public bool IsCancelInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActionQueue.Instance.ClearQueue();
            EventBus.Publish(new HideIndicatorEvent());
            EventBus.Publish(new CursorEventData(cursorType.Idle));
            return true;
        }
        return false;
    }

    public bool IsStopRequested()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            ActionQueue.Instance.ClearQueue();
            ActionQueue.Instance.EnqueueAction(StateType.Idle);
            EventBus.Publish(new CursorEventData(cursorType.Idle));
            EventBus.Publish(new HideIndicatorEvent());
            return true;
        }        
        return false;
    }
}
