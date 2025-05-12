using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector3 GetInputMousePosition()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            return hit.point;
        }        
        return Vector3.zero;
    }
}
