using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MpBar : MonoBehaviour
{
    Image mpBar;    

    [SerializeField] private float minY = 0f;
    [SerializeField] private float maxY = 1f;
    private void Awake()
    {
        mpBar = GetComponent<Image>();        
    }

    private void OnEnable()
    {
        EventBus.Subscribe<MpChangeEvent>(OnChangeMp);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<MpChangeEvent>(OnChangeMp);
    }

    private void OnChangeMp(MpChangeEvent changeEvent)
    {
        float ratio = Mathf.Clamp01(changeEvent.CurrentMP / changeEvent.MaxMp);        

        mpBar.fillAmount = ratio;
    }
}
