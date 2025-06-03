using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class HpGlobe : MonoBehaviour
{
    Image manaGlobe;
    TextMeshProUGUI hpText;

    [SerializeField] private float minY = 0f;
    [SerializeField] private float maxY = 100f;

    GameObject owner;
    private void Awake()
    {
        manaGlobe = GetComponent<Image>();
        GameObject.Find("HpText").TryGetComponent<TextMeshProUGUI>(out hpText);
    }

    private void OnEnable()
    {
        EventBus.Subscribe<HpChangeEvent>(OnChangeHPEvent);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<HpChangeEvent>(OnChangeHPEvent);
    }

    public void SetTarget(GameObject newOnwer)
    {
        owner = newOnwer;
    }

    private void OnChangeHPEvent(HpChangeEvent changeEvent)
    {
        if (changeEvent.Publisher != owner) return;

        float ratio = Mathf.Clamp01(changeEvent.CurrentHp / changeEvent.MaxHp);
        // 위치 보간
        float yPos = Mathf.Lerp(minY, maxY, ratio);

        manaGlobe.rectTransform.localPosition = new Vector3(0f, yPos, 0f);

        if (yPos < 50f)
        {
            hpText.color = Color.black;
        }
        else
        {
            hpText.color = Color.white;
        }

        hpText.text = $"{Mathf.FloorToInt(yPos)} / {100}";
    }
}
