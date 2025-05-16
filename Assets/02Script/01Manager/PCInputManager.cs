using System;
using System.Collections.Generic;
using UnityEngine;

public class PCInputManager : ManagerBase, IInputHandler
{
    public static Action<Vector3> OnMoveEvent;
    public event Action<int> OnSkillInput; // int���� �Ű������� �ϴ� �޼ҵ���� ���ε�. 
                                           // ������ ��ų��� ���ε�. => � Ű�� �ԷµǾ����� �ش� Ű�� ���ε��� �� ��ų�� �ִٸ� �ش� ��ų ��� ��û �̺�Ʈ
                                           // PlayerSkillManager���� ���ε�.
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
                Debug.Log("Ÿ�� ����");
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
            if (Input.GetKeyDown(binding.Key)) // Key�� Dictionary�� key��
            {
                OnSkillInput?.Invoke(binding.Value); // Value�� Dictionary�� Value��
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
            
            Debug.Log("Ÿ�� ����");
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



    private Dictionary<KeyCode, int> keySkillBindings = new Dictionary<KeyCode, int>(); // KeyCode�� ���� int�� ���ε�(C++�� map)

    public void BindKeyToSkill(KeyCode key, int skillIndex)
    {
        keySkillBindings[key] = skillIndex; // KeyCode�� int�� �־���
    }
}
