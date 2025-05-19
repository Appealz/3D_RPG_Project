using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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

    private Dictionary<KeyCode, SkillType> keySkillBindings = new Dictionary<KeyCode, SkillType>();

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if (Input.GetMouseButtonDown(1))
        {
            GetInputClick();
        }

        foreach (var binding in keySkillBindings)
        {
            if (Input.GetKeyDown(binding.Key))
            {
                Debug.Log($"{binding.Key}눌림, 스킬 입력");
                OnSkillInput?.Invoke(binding.Value);
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
        }        
    }
       

    public void BindKeyToSkill(KeyCode key, SkillType skillType)
    {
        Debug.Log("인풋 매니저 스킬 바인딩");
        keySkillBindings[key] = skillType;
    }
}



