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
        
        // ���콺 ��Ŭ��
        if (Input.GetMouseButtonDown(1))
        {
            GetInputClick();
            OnReadyToAttack?.Invoke(false);
        }

        // ��ų Ű �Է�
        foreach (var binding in keySkillBindings)
        {
            if (Input.GetKeyDown(binding.Key))
            {
                Debug.Log($"{binding.Key}����, ��ų �Է�");                
                OnReadyToAttack?.Invoke(true);                    
                currentReadySkill = binding.Value;                
            }
        }

        // ��ų �غ� ����
        if (Input.GetMouseButtonDown(0) && currentReadySkill.HasValue)
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {
                Debug.Log("Ÿ�� ����");                
                OnReadyToAttack?.Invoke(false);
                OnSkillInput?.Invoke(currentReadySkill.Value);
                currentReadySkill = null;
            }            
        }

        // ����(A)Ű �Է�
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnReadyToAttack?.Invoke(true);
            isAttackOn = true;
        }

        // ����(S)Ű �Է�
        if(Input.GetKeyDown(KeyCode.S))
        {
            OnStop?.Invoke(StateType.Idle);
        }

        // ����Ű�� �ԷµǾ�������
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
            Debug.Log("Ÿ�� ����");
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
        Debug.Log("��ǲ �Ŵ��� ��ų ���ε�");
        keySkillBindings[key] = skillType;
    }
}



