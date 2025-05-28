using System.Collections.Generic;
using UnityEngine;
public enum cursorType
{
    Idle,
    Attack,
    Aim,
}

public class CursorManager : ManagerBase
{
    public Texture2D initCursor;
    public Texture2D attackCursor;
    public Texture2D AclickCursor;
    public Texture2D skillCursor;
    private Texture2D currentCursor;

    private bool readyToAttack;
    private bool targetCheck;

    private Dictionary<cursorType, Texture2D> cursorTypes = new Dictionary<cursorType, Texture2D>();
    private cursorType currentCursorType;

    private void Awake()
    {
        Cursor.SetCursor(initCursor, Vector2.zero, CursorMode.Auto);
        //PCInputManager.OnReadyToAttackCursor += ReadyToAttack;
        cursorTypes[cursorType.Idle] = initCursor;
        cursorTypes[cursorType.Attack] = attackCursor;
        cursorTypes[cursorType.Aim] = AclickCursor;

        EventBus.Subscribe<CursorEventData>(CursorStateChange);
    }

    public override void CustomUpdate()
    {
        base.CustomUpdate();
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, LayerMask.GetMask("Enemy")))
        {
            //Debug.Log("타겟 추적");
            //TargetCheck(true);
            if(cursorType.Idle == currentCursorType)
            {
                CursorTypeChange(cursorType.Attack);
            }            
        }
        else
        {
            //TargetCheck(false);
            if(cursorType.Attack == currentCursorType)
            {
                CursorTypeChange(cursorType.Idle);
            }
            CursorTypeChange(currentCursorType);
        }
    }

    public void CursorStateChange(CursorEventData eventData)
    {        
        CursorTypeChange(eventData.CursorType);
    }

    public void CursorTypeChange(cursorType newType)
    {
        if (currentCursorType != newType)
        {
            Debug.Log("커서 변경");
            Cursor.SetCursor(cursorTypes[newType], Vector2.zero, CursorMode.Auto);            
            currentCursorType = newType; // 현재 커서 상태를 업데이트
        }
    }

    //public void ChangeCursor()
    //{
    //    Texture2D targetCursor;

    //    // 만약 공격 준비 중이거나, 적 타겟이 마우스 아래에 있을 경우 → 공격 커서로 변경
    //    if (targetCheck && !readyToAttack)
    //    {
    //        targetCursor = attackCursor;
    //    }
    //    else if(readyToAttack)
    //    {
    //        targetCursor = AclickCursor;
    //    }
    //    else
    //    {
    //        // 그렇지 않으면 기본 커서로 설정
    //        targetCursor = initCursor;
    //    }

    //    // 현재 설정된 커서와 새로 설정하려는 커서가 다를 경우에만 SetCursor 호출        
    //    if (currentCursor != targetCursor)
    //    {
    //        Cursor.SetCursor(targetCursor, Vector2.zero, CursorMode.Auto);
    //        currentCursor = targetCursor; // 현재 커서 상태를 업데이트
    //    }
    //}

    //public void ReadyToAttack(bool isOn)
    //{
    //    readyToAttack = isOn;
    //    ChangeCursor();
    //}

    //public void TargetCheck(bool isOn)
    //{
    //    targetCheck = isOn;
    //    ChangeCursor();
    //}
}
