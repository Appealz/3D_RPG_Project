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

    public void OnSkillBtnClick()
    {
        //EventBus.Publish(new SkillAvailablityEvent(skillTypeBtn, OnSkillAvailablityChecked));
        EventBus.Publish(new SkillPreparedEvent(skillTypeBtn));
    }
}
    

