using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.GridLayoutGroup;

public class MpGlobe : MonoBehaviour
{
    #region _Field_
    Image manaGlobe;
    TextMeshProUGUI mpText;

    GameObject owner;

    [SerializeField] private float minY = -200f;
    [SerializeField] private float maxY = 0f;
    #endregion
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
    public void SetTarget(GameObject newOnwer)
    {
        owner = newOnwer;
    }
    private void OnChangeMp(MpChangeEvent changeEvent)
    {
        float ratio = Mathf.Clamp01(changeEvent.CurrentMP / changeEvent.MaxMp);
        // 위치 보간
        float yPos = Mathf.Lerp(minY, maxY, ratio);
        manaGlobe.rectTransform.localPosition = new Vector3(0f, yPos, 0f);

        if(yPos < (minY - maxY) / 2f)
        {
            mpText.color = Color.black;
        }
        else
        {
            mpText.color = Color.white;
        }

        mpText.text = $"{Mathf.FloorToInt(changeEvent.CurrentMP)} / {100}";
    }
}


