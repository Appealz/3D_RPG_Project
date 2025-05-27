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

    public static event Func<SkillType, bool> OnSkillAvailablity;

    //public event Action<SkillType> OnSkillButtonInput;
    //public static event Action OnSkillActive;
        
    // CursorManager
    public static event Action<bool> OnReadyToAttackCursor;
    public static event Action<StateType> OnStop;

    // SkillManager
    private Dictionary<KeyCode, SkillType> keySkillBindings = new Dictionary<KeyCode, SkillType>();

    private bool isAttackOn;
    //private bool isSkillReady;

    private SkillType? currentReadySkill = null;

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        
        // 마우스 우클릭
        if (Input.GetMouseButtonDown(1))
        {
            GetInputClick();
            OnReadyToAttackCursor?.Invoke(false);
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
                EventBus.Publish(new SkillActivatedEvent(currentReadySkill.Value));
                currentReadySkill = null;
                OnReadyToAttackCursor?.Invoke(false);
            }            
        }

        // 스킬 키 입력
        foreach (var binding in keySkillBindings)
        {
            if (Input.GetKeyDown(binding.Key))
            {
                if (!OnSkillAvailablity.Invoke(binding.Value))
                {
                    Debug.Log("스킬 사용 불가");
                    return;
                }
                if(binding.Value != SkillType.E_Skill)
                {
                    OnReadyToAttackCursor?.Invoke(true);
                }                
                //OnSkillButtonInput?.Invoke(binding.Value);
                currentReadySkill = binding.Value;
                EventBus.Publish(new SkillPreparedEvent(currentReadySkill.Value));
            }
        }



        // 공격(A)키 입력
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnReadyToAttackCursor?.Invoke(true);
            isAttackOn = true;
        }

        // 정지(S)키 입력
        if(Input.GetKeyDown(KeyCode.S))
        {
            OnStop?.Invoke(StateType.Idle);
            OnReadyToAttackCursor?.Invoke(false);
        }

        // 공격키가 입력되어있을때
        if (isAttackOn)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                GetInputClick();
                OnReadyToAttackCursor?.Invoke(false);
            }
        }

    }

    public void GetInputClick()
    {   
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {
            //Debug.Log("타겟 추적");
            //OnMouseTargetClick?.Invoke(hit.transform);
            EventBus.Publish(new TargetSelectEvent(hit.transform));
        }
        else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {            
            GameObject obj = ObjectPoolManager.Instance.pool[1].PopObj();
            obj.transform.position = hit.point;         
            //OnMouseMoveClick?.Invoke(hit.point);
            OnReadyToAttackCursor?.Invoke(false);
            EventBus.Publish(new TargetPositionEvent(hit.point));
        }
        isAttackOn = false;
    }
    

    public void BindKeyToSkill(KeyCode key, SkillType skillType)
    {
        Debug.Log("인풋 매니저 스킬 바인딩");
        keySkillBindings[key] = skillType;
    }
}



