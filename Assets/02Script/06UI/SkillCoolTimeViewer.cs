using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimeViewer : MonoBehaviour
{
    private Image coolTimeOverlay;
    [SerializeField] private SkillType skillType;


    private void OnEnable()
    {
        coolTimeOverlay = GetComponent<Image>();
        coolTimeOverlay.fillAmount = 0f;
        EventBus.Subscribe<SkillCoolDownEvent>(OnCoolTimeStarted);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<SkillCoolDownEvent>(OnCoolTimeStarted);
    }

    private void OnCoolTimeStarted(SkillCoolDownEvent coolDownStruc)
    {
        if(coolDownStruc.Type == skillType)
        {
            //Debug.Log("ƒ≈∏¿” Ω√¿€");
            StartCoroutine(StartCoolTimeCoroutine(coolDownStruc.CoolDownTime));
        }
    }

    IEnumerator StartCoolTimeCoroutine(float duration)
    {
        float time = 0f;
        while(time < duration)
        {
            time += Time.deltaTime;
            coolTimeOverlay.fillAmount = 1f - (time/duration);
            yield return null;
        }
        coolTimeOverlay.fillAmount = 0f;
    }
}
