using UnityEngine;

public class CursorManager : ManagerBase
{
    public Texture2D initCursor;
    public Texture2D attackCursor;
    public Texture2D AclickCursor;
    private Texture2D currentCursor;

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
            //Debug.Log("Ÿ�� ����");
            TargetCheck(true);
        }
        else
        {
            TargetCheck(false);
        }
    }
    public void ChangeCursor()
    {
        Texture2D targetCursor;

        // ���� ���� �غ� ���̰ų�, �� Ÿ���� ���콺 �Ʒ��� ���� ��� �� ���� Ŀ���� ����
        if (targetCheck && !readyToAttack)
        {
            targetCursor = attackCursor;
        }
        else if(readyToAttack)
        {
            targetCursor = AclickCursor;
        }
        else
        {
            // �׷��� ������ �⺻ Ŀ���� ����
            targetCursor = initCursor;
        }

        // ���� ������ Ŀ���� ���� �����Ϸ��� Ŀ���� �ٸ� ��쿡�� SetCursor ȣ��        
        if (currentCursor != targetCursor)
        {
            Cursor.SetCursor(targetCursor, Vector2.zero, CursorMode.Auto);
            currentCursor = targetCursor; // ���� Ŀ�� ���¸� ������Ʈ
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
