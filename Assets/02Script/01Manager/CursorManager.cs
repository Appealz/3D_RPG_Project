using UnityEngine;

public class CursorManager : ManagerBase
{
    public Texture2D initCursor;
    public Texture2D attackCursor;

    private bool readyToAttack;
    private bool targetCheck;

    private void Awake()
    {
        Cursor.SetCursor(initCursor, Vector2.zero, CursorMode.Auto);
        PCInputManager.OnReadyToAttack += ReadyToAttack;
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {
            Debug.Log("Å¸°Ù ÃßÀû");
            TargetCheck(true);
        }
        else
        {
            TargetCheck(false);
        }
    }
    public void ChangeCursor()
    {
        if (readyToAttack)
        {
            Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
        }
        else if (targetCheck)
        {
            Cursor.SetCursor(attackCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(initCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    public void ReadyToAttack(bool isOn)
    {
        readyToAttack = isOn;
        ChangeCursor();
    }

    public void TargetCheck(bool isOn)
    {
        targetCheck = isOn;
        ChangeCursor();
    }
}
