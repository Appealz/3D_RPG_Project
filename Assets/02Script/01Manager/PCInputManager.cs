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
    public static event Action<Vector3> OnMouseMoveClick;
    public static event Action<Transform> OnMouseTargetClick;
    public event Action<SkillType> OnSkillInput;

    public static event Action<bool> OnReadyToAttack;
    public static event Action<StateType> OnStop;

    private Dictionary<KeyCode, SkillType> keySkillBindings = new Dictionary<KeyCode, SkillType>();

    private bool isAttackOn;
    private bool isQkeyOn;

    private SkillType? currentReadySkill = null;
    public override void CustomUpdate()
    {
        base.CustomUpdate();
        
        // 마우스 우클릭
        if (Input.GetMouseButtonDown(1))
        {
            GetInputClick();
            OnReadyToAttack?.Invoke(false);
        }

        // 스킬 키 입력
        foreach (var binding in keySkillBindings)
        {
            if (Input.GetKeyDown(binding.Key))
            {
                Debug.Log($"{binding.Key}눌림, 스킬 입력");                
                OnReadyToAttack?.Invoke(true);                    
                currentReadySkill = binding.Value;                
            }
        }

        // 스킬 준비 상태
        if (Input.GetMouseButtonDown(0) && currentReadySkill.HasValue)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {
                Debug.Log("타겟 추적");                
                OnReadyToAttack?.Invoke(false);
                OnSkillInput?.Invoke(currentReadySkill.Value);
                currentReadySkill = null;
            }            
        }

        // 공격(A)키 입력
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnReadyToAttack?.Invoke(true);
            isAttackOn = true;
        }

        // 정지(S)키 입력
        if(Input.GetKeyDown(KeyCode.S))
        {
            OnStop?.Invoke(StateType.Idle);
        }

        // 공격키가 입력되어있을때
        if (isAttackOn)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                GetInputClick();
                OnReadyToAttack?.Invoke(false);
            }
        }

    }

    public void GetInputClick()
    {   
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {
            Debug.Log("타겟 추적");
            OnMouseTargetClick?.Invoke(hit.transform);
        }
        else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {            
            GameObject obj = ObjectPoolManager.Instance.pool[1].PopObj();
            obj.transform.position = hit.point;         
            OnMouseMoveClick?.Invoke(hit.point);
            OnReadyToAttack?.Invoke(false);
        }
        isAttackOn = false;
    }
    

    public void BindKeyToSkill(KeyCode key, SkillType skillType)
    {
        Debug.Log("인풋 매니저 스킬 바인딩");
        keySkillBindings[key] = skillType;
    }
}



