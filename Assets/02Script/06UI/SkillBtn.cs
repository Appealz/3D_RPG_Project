using UnityEngine;
using UnityEngine.UI;

public class SkillBtn : MonoBehaviour
{
    [SerializeField]
    private SkillType skillTypeBtn;

    Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();

        btn.onClick.AddListener(OnSkillBtnClick);
    }

    private void OnSkillAvailablityChecked(bool canUse)
    {
        if (canUse)
        {
            EventBus.Publish(new CursorEventData(cursorType.Aim));
            EventBus.Publish(new SkillPreparedEvent(skillTypeBtn));
        }
        else
        {
            Debug.Log("스킬 사용 불가 - 쿨타임 또는 MP 부족");
        }
    }


    public void OnSkillBtnClick()
    {
        //EventBus.Publish(new SkillAvailablityEvent(skillTypeBtn, (canUse) =>
        //{
        //    if (canUse)
        //    {
        //        // 스킬 준비 이벤트 발행
        //        EventBus.Publish(new SkillPreparedEvent(skillTypeBtn));
        //    }
        //    else
        //    {
        //        Debug.Log("스킬 사용 불가 - 쿨타임 또는 MP 부족");
        //    }
        //}));

        //EventBus.Publish(new SkillPreparedEvent(skillTypeBtn));

        EventBus.Publish(new SkillAvailablityEvent(skillTypeBtn, OnSkillAvailablityChecked));
    }
}
    

