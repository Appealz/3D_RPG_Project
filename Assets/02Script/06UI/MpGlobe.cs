using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MpGlobe : MonoBehaviour
{
    Image manaGlobe;
    TextMeshProUGUI mpText;

    [SerializeField] private float minY = 0f;
    [SerializeField] private float maxY = 100f;
    private void Awake()
    {
        manaGlobe = GetComponent<Image>();
        GameObject.Find("MpText").TryGetComponent<TextMeshProUGUI>(out mpText);
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

        if(yPos < 50f)
        {
            mpText.color = Color.black;
        }
        else
        {
            mpText.color = Color.white;
        }

        mpText.text = $"{Mathf.FloorToInt(yPos)} / {100}";
    }
}
