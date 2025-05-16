using System;
using System.Collections.Generic;
using UnityEngine;

public class PCInputManager : ManagerBase, IInputHandler
{
    public static Action<Vector3> OnMoveEvent;
    public event Action<int> OnSkillInput; // int값을 매개변수로 하는 메소드들을 바인딩. 
                                           // 각각의 스킬들과 바인딩. => 어떤 키가 입력되었을때 해당 키에 바인딩이 된 스킬이 있다면 해당 스킬 사용 요청 이벤트
                                           // PlayerSkillManager에서 바인딩.
    public ClickReturn GetInputClick()
    {
        ClickReturn clickReturn = new ClickReturn();
        if (Input.GetMouseButtonDown(1))
        {            
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
            {
                clickReturn.pos = Vector3.zero;
                clickReturn.targetTrans = hit.transform;
                Debug.Log("타겟 추적");
            }
            else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                clickReturn.pos = hit.point;
                clickReturn.targetTrans = null;
                GameObject obj = ObjectPoolManager.Instance.pool[1].PopObj();
                obj.transform.position = hit.point;
            }

        }
        return clickReturn;
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if(Input.GetMouseButtonDown(1))
        {
            
            GetInputClick2();
        }

        foreach (var binding in keySkillBindings)
        {
            if (Input.GetKeyDown(binding.Key)) // Key는 Dictionary의 key값
            {
                OnSkillInput?.Invoke(binding.Value); // Value는 Dictionary의 Value값
            }
        }
    }

    public ClickReturn GetInputClick2()
    {        
        ClickReturn clickReturn = new ClickReturn();
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {
            clickReturn.pos = Vector3.zero;
            clickReturn.targetTrans = hit.transform;
            
            Debug.Log("타겟 추적");
            OnMoveEvent?.Invoke(clickReturn.targetTrans.position);
        }
        else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            clickReturn.pos = hit.point;
            clickReturn.targetTrans = null;
            GameObject obj = ObjectPoolManager.Instance.pool[1].PopObj();
            obj.transform.position = hit.point;
            OnMoveEvent?.Invoke(clickReturn.pos);
        }
        return clickReturn;
    }



    private Dictionary<KeyCode, int> keySkillBindings = new Dictionary<KeyCode, int>(); // KeyCode에 따라서 int값 바인딩(C++의 map)

    public void BindKeyToSkill(KeyCode key, int skillIndex)
    {
        keySkillBindings[key] = skillIndex; // KeyCode에 int값 넣어줌
    }
}
