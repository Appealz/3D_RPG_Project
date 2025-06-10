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


    // 마우스 이동 인디케이터 표시
    private void CreateMouseIndicator(RaycastHit hit)
    {
        GameObject obj = ObjectPoolManager.Instance.pool[1].PopObj();
        Vector3 setPoint = new Vector3(hit.point.x, hit.point.y + 0.2f, hit.point.z);
        obj.transform.position = setPoint;
    }
    
    // 스킬 바인딩
    public void BindKeyToSkill(KeyCode key, SkillType skillType)
    {
        keySkillBindings[key] = skillType;
    }

    // 마우스 우클릭(이동)
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

    // 마우스 우클릭(타겟 지정(공격))
    public bool TryGetRightClickTarget(out Transform target)
    {
        target = null;
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {                
                EventBus.Publish(new CursorEventData(cursorType.Idle));
                EventBus.Publish(new HideIndicatorEvent());
                target = hit.transform;
                return true;
            }
        }
        return false;
    }

    // 공격(키보드 A) 입력
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

    // A가 입력되어있는 상태일때 타겟 지정(공격) => 마우스 좌,우 클릭 관계없이 타겟 지정
    public bool TryGetAttackTargetClick(out Transform target)
    {
        target = null;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {                
                EventBus.Publish(new CursorEventData(cursorType.Idle));
                EventBus.Publish(new HideIndicatorEvent());
                target = hit.transform;
                return true;
            }
        }
        return false;
    }

    // A가 입력되어있는 상태일때 이동 => 마우스 좌,우 클릭 관계없이 타겟이 지정되지 않았을때 해당 위치로 이동
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

    // 스킬(ex. Q W E R) 입력 => 바인딩 된 스킬 키 중 입력된 키에 바인딩된 스킬 타입 반환.
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

    // 스킬이 준비된 상태
    public bool IsSkillReady(out SkillType skillType)
    {
        skillType = SkillType.None;
        return false;
    }

    // 타겟 스킬이 입력된 상태에서 타겟 지정
    public bool TryGetSkillTarget(out Transform target)
    {
        target = null;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {                
                EventBus.Publish(new CursorEventData(cursorType.Idle));
                EventBus.Publish(new HideIndicatorEvent());
                target = hit.transform;
                return true;
            }
        }
        return false;
    }

    // 논 타겟 스킬이 입력된 상태에서 마우스의 위치 반환
    public bool TryGetSkillDirection(out Vector3 direction)
    {
        direction = Vector3.zero;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {                
                EventBus.Publish(new CursorEventData(cursorType.Idle));
                EventBus.Publish(new HideIndicatorEvent());
                direction = hit.point;
                return true;
            }
        }
        return false;
    }

    // 논 타겟 스킬이 입력된 상태에서 해당 위치 지정
    public bool TryGetSkillPosition(out Vector3 position)
    {
        position = Vector3.zero;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {                
                EventBus.Publish(new CursorEventData(cursorType.Idle));
                EventBus.Publish(new HideIndicatorEvent());
                position = hit.point;
                return true;
            }
        }
        return false;
    }

    // 취소(ESC) 키 입력
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

    // 정지(S 키) 입력
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
