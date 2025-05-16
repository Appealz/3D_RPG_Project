using System;
using System.Collections.Generic;
using UnityEngine;

public class PCInputManager : ManagerBase, IInputHandler
{
    public static event Action<Vector3> OnMouseMoveClick;
    public static event Action<Transform> OnMouseTargetClick;
    public event Action<int> OnSkillInput;

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        if (Input.GetMouseButtonDown(1))
        {
            GetInputClick();
        }

        //foreach (var binding in keySkillBindings)
        //{
        //    if (Input.GetKeyDown(binding.Key)) // Key�� Dictionary�� key��
        //    {
        //        OnSkillInput?.Invoke(binding.Value); // Value�� Dictionary�� Value��
        //    }
        //}
    }

    public void GetInputClick()
    {        
        ClickReturn clickReturn = new ClickReturn();
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {            
            clickReturn.targetTrans = hit.transform;            
            Debug.Log("Ÿ�� ����");
            OnMouseTargetClick?.Invoke(clickReturn.targetTrans);            
        }
        else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            clickReturn.pos = hit.point;            
            GameObject obj = ObjectPoolManager.Instance.pool[1].PopObj();
            obj.transform.position = hit.point;
            OnMouseMoveClick?.Invoke(clickReturn.pos);
        }        
    }

    private Dictionary<KeyCode, int> keySkillBindings = new Dictionary<KeyCode, int>(); // KeyCode�� ���� int�� ���ε�(C++�� map)

    public void BindKeyToSkill(KeyCode key, int skillIndex)
    {
        keySkillBindings[key] = skillIndex; // KeyCode�� int�� �־���
    }
}




//public ClickReturn GetInputClick2()
//{
//    ClickReturn clickReturn = new ClickReturn();
//    if (Input.GetMouseButtonDown(1))
//    {
//        RaycastHit hit;

//        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
//        {
//            clickReturn.pos = Vector3.zero;
//            clickReturn.targetTrans = hit.transform;
//            Debug.Log("Ÿ�� ����");
//        }
//        else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
//        {
//            clickReturn.pos = hit.point;
//            clickReturn.targetTrans = null;
//            GameObject obj = ObjectPoolManager.Instance.pool[1].PopObj();
//            obj.transform.position = hit.point;
//        }

//    }
//    return clickReturn;
//}