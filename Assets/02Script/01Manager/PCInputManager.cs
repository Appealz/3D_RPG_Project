using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UIElements;

public enum SkillType
{
    Q_Skill,
    W_Skill,
    E_Skill,
    R_Skill,
}

public class PCInputManager : ManagerBase, IInputHandler
{
    //public static event Action<Vector3> OnMouseMoveClick;
    //public static event Action<Transform> OnMouseTargetClick;

    //public static event Func<SkillType, bool> OnSkillAvailablity;

    //public event Action<SkillType> OnSkillButtonInput;
    //public static event Action OnSkillActive;
        
    // CursorManager
    //public static event Action<bool> OnReadyToAttackCursor;
    public static event Action<StateType> OnStop;

    // SkillManager
    private Dictionary<KeyCode, SkillType> keySkillBindings = new Dictionary<KeyCode, SkillType>();

    private bool isAttackOn;
    //private bool isSkillReady;

    private SkillType? currentReadySkill = null;
    private void OnEnable()
    {
        EventBus.Subscribe<SkillPreparedEvent>(OnSkillPrepared);        
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<SkillPreparedEvent>(OnSkillPrepared);
    }



    public override void CustomUpdate()
    {
        base.CustomUpdate();
        
        // 마우스 우클릭
        if (Input.GetMouseButtonDown(1))
        {
            GetInputClick();
            //OnReadyToAttackCursor?.Invoke(false);
            EventBus.Publish(new CursorEventData(cursorType.Idle));
            if (currentReadySkill.HasValue)
            {
                currentReadySkill = null;
            }
        }

        // 마우스 좌클릭
        if (Input.GetMouseButtonDown(0))
        {
            if(currentReadySkill.HasValue)
            {
                //OnSkillActive?.Invoke();
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
                {
                    //Debug.Log("타겟 추적");
                    //OnMouseTargetClick?.Invoke(hit.transform);
                    EventBus.Publish(new SkillTargetSelectedEvent(hit.transform));
                    if(currentReadySkill.Value == SkillType.Q_Skill)
                    {
                        EventBus.Publish(new SkillActivatedEvent(currentReadySkill.Value));
                    }
                    if (currentReadySkill.Value == SkillType.W_Skill)
                    {
                        EventBus.Publish(new SkillTargetPositionEvent(hit.point));
                        EventBus.Publish(new SkillActivatedEvent(currentReadySkill.Value));
                    }
                }
                else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if(currentReadySkill.Value == SkillType.W_Skill)
                    {
                        EventBus.Publish(new SkillTargetPositionEvent(hit.point));
                        EventBus.Publish(new SkillActivatedEvent(currentReadySkill.Value));
                    }
                }
                
                currentReadySkill = null;
                //OnReadyToAttackCursor?.Invoke(false);
                //EventBus.Publish(new CursorEventData(cursorType.Idle));
            }
        }

        // 스킬 키 입력
        foreach (var binding in keySkillBindings)
        {
            if (Input.GetKeyDown(binding.Key))
            {
                currentReadySkill = binding.Value;
                EventBus.Publish(new SkillAvailablityEvent(binding.Value, OnKeySkillAvailablityChecked));        
            }
        }

        // 공격(A)키 입력
        if (Input.GetKeyDown(KeyCode.A))
        {
            //OnReadyToAttackCursor?.Invoke(true);
            EventBus.Publish(new CursorEventData(cursorType.Aim));
            isAttackOn = true;
        }

        // 정지(S)키 입력
        if(Input.GetKeyDown(KeyCode.S))
        {
            OnStop?.Invoke(StateType.Idle);
            //OnReadyToAttackCursor?.Invoke(false);
            EventBus.Publish(new CursorEventData(cursorType.Idle));
        }

        // 공격키가 입력되어있을때
        if (isAttackOn)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                GetInputClick();
                //OnReadyToAttackCursor?.Invoke(false);
                EventBus.Publish(new CursorEventData(cursorType.Idle));
            }
        }
    }

    private void OnKeySkillAvailablityChecked(bool canUse)
    {
        if (!canUse)
        {
            Debug.Log("스킬 사용 불가");
            return;
        }
        else
        {
            EventBus.Publish(new SkillPreparedEvent(currentReadySkill.Value));
        }
        //if (currentReadySkill != SkillType.E_Skill)
        //{
        //    //OnReadyToAttackCursor?.Invoke(true);
        //}

        
    }

    private void OnSkillPrepared(SkillPreparedEvent preparedSkillType)
    {
        currentReadySkill = preparedSkillType.SkillType;

        // 커서 전환까지 여기서 처리하면 버튼, 키보드 모두 동일 로직
        //OnReadyToAttackCursor?.Invoke(true);
    }
    public void GetInputClick()
    {   
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {
            //Debug.Log("타겟 추적");
            //OnMouseTargetClick?.Invoke(hit.transform);
            EventBus.Publish(new TargetSelectEvent(hit.transform));
            ActionQueue.Instance.ClearQueue(); // 혹시 모를 꼬임 방지
            ActionQueue.Instance.EnqueueAction(StateType.Attack); // 명시적으로 Attack 큐잉
        }
        else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {            
            GameObject obj = ObjectPoolManager.Instance.pool[1].PopObj();
            obj.transform.position = hit.point;         
            //OnMouseMoveClick?.Invoke(hit.point);
            //OnReadyToAttackCursor?.Invoke(false);
            EventBus.Publish(new CursorEventData(cursorType.Idle));
            EventBus.Publish(new TargetPositionEvent(hit.point));
        }
        isAttackOn = false;
    }
    

    public void BindKeyToSkill(KeyCode key, SkillType skillType)
    {
        //Debug.Log("인풋 매니저 스킬 바인딩");
        keySkillBindings[key] = skillType;
    }
}



