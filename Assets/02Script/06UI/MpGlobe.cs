using UnityEngine;
using UnityEngine.UI;

public class MpGlobe : MonoBehaviour
{
    Image manaGlobe;
    [SerializeField] private float minY = 0f;
    [SerializeField] private float maxY = 100f;
    private void Awake()
    {
        manaGlobe = GetComponent<Image>();
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
        // 위치 보간
        float yPos = Mathf.Lerp(minY, maxY, ratio);

        manaGlobe.rectTransform.localPosition = new Vector3(0f, yPos, 0f);        
    }
}
