using UnityEngine;

public class PCInputManager : MonoBehaviour, IInputHandler
{ 
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
                Debug.Log("Å¸°Ù ÃßÀû");
            }
            else if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                clickReturn.pos = hit.point;
                clickReturn.targetTrans = null;
            }

        }
        return clickReturn;
    }
}
