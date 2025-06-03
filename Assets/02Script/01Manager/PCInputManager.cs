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
    // CursorManager    
    public static event Action<StateType> OnStop;

    // SkillManager
    private Dictionary<KeyCode, SkillType> keySkillBindings = new Dictionary<KeyCode, SkillType>();

    private bool isAttackOn;    

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
            EventBus.Publish(new CursorEventData(cursorType.Idle));
            EventBus.Publish(new HideIndicatorEvent());
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
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
                {                     
                    if(currentReadySkill.Value == SkillType.Q_Skill)
                    {
                        EventBus.Publish(new SkillTargetSelectedEvent(hit.transform));
                        EventBus.Publish(new SkillActivatedEvent(currentReadySkill.Value));
                    }
                    if (currentReadySkill.Value == SkillType.W_Skill)
                    {
                        EventBus.Publish(new SkillTargetPositionEvent(hit.point));
                        EventBus.Publish(new SkillActivatedEvent(currentReadySkill.Value));
                    }
                    if (currentReadySkill.Value == SkillType.R_Skill)
                    {
                        EventBus.Publish(new SkillTargetPositionEvent(hit.point));
                        EventBus.Publish(new TargetPositionEvent(hit.point));
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
                    if (currentReadySkill.Value == SkillType.R_Skill)
                    {
                        EventBus.Publish(new SkillTargetPositionEvent(hit.point));
                        EventBus.Publish(new TargetPositionEvent(hit.point));
                        EventBus.Publish(new SkillActivatedEvent(currentReadySkill.Value));
                    }
                }

                currentReadySkill = null;
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
            EventBus.Publish(new CursorEventData(cursorType.Aim));
            EventBus.Publish(new indicatorEvent(IndicatorType.Circle, Vector3.zero, 5f));
            isAttackOn = true;
        }

        // 정지(S)키 입력
        if(Input.GetKeyDown(KeyCode.S))
        {
            OnStop?.Invoke(StateType.Idle);            
            EventBus.Publish(new CursorEventData(cursorType.Idle));
        }

        // 공격키가 입력되어있을때
        if (isAttackOn)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                GetInputClick();
                EventBus.Publish(new HideIndicatorEvent());
                EventBus.Publish(new CursorEventData(cursorType.Idle));
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EventBus.Publish(new HideIndicatorEvent());
                EventBus.Publish(new CursorEventData(cursorType.Idle));
            }
        }

        if(currentReadySkill.HasValue)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EventBus.Publish(new HideIndicatorEvent());
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
    }

    private void OnSkillPrepared(SkillPreparedEvent preparedSkillType)
    {
        currentReadySkill = preparedSkillType.SkillType;
    }

    public void GetInputClick()
    {   
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {
            EventBus.Publish(new TargetSelectEvent(hit.transform));
            ActionQueue.Instance.ClearQueue(); 
            ActionQueue.Instance.EnqueueAction(StateType.Attack);
        }
        else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {            
            GameObject obj = ObjectPoolManager.Instance.pool[1].PopObj();
            Vector3 setPoint = new Vector3(hit.point.x , hit.point.y + 0.2f, hit.point.z);
            obj.transform.position = setPoint;
            EventBus.Publish(new CursorEventData(cursorType.Idle));
            EventBus.Publish(new TargetPositionEvent(hit.point));
            ActionQueue.Instance.ClearQueue();
        }
        isAttackOn = false;
    }
    

    public void BindKeyToSkill(KeyCode key, SkillType skillType)
    {        
        keySkillBindings[key] = skillType;
    }
}



