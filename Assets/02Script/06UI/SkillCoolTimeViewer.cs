using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTimeViewer : MonoBehaviour
{
    #region _Field_
    private Image coolTimeOverlay;
    private Image coolTimeBackground;

    private TextMeshProUGUI coolTimeText;
    [SerializeField] private SkillType skillType;
    #endregion
    private void OnEnable()
    {
        #region _reference_
        coolTimeOverlay = FindObjectTransform.FindChildTransform(transform, "Marker").GetComponent<Image>();
        coolTimeOverlay.fillAmount = 0f;

        coolTimeBackground = FindObjectTransform.FindChildTransform(transform, "CoolTimeBackGround").GetComponent<Image>();
        coolTimeBackground.gameObject.SetActive(false);

        coolTimeText = GetComponentInChildren<TextMeshProUGUI>();        
        coolTimeText.enabled = false;
        #endregion
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
            coolTimeText.enabled = true;
            coolTimeBackground.gameObject.SetActive(true);
            StartCoroutine(StartCoolTimeCoroutine(coolDownStruc.CoolDownTime));
        }
    }

    IEnumerator StartCoolTimeCoroutine(float duration)
    {
        float time = 0f;
        while(time < duration)
        {
            time += Time.deltaTime;
            coolTimeText.text = $"{Mathf.Round(duration-time)}";
            coolTimeOverlay.fillAmount = 1f - (time/duration);
            yield return null;
        }
        coolTimeBackground.gameObject.SetActive(false);
        coolTimeText.enabled = false;
        coolTimeOverlay.fillAmount = 0f;
    }
}
