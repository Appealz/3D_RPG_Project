using UnityEngine;
using UnityEngine.UI;

public class SkillMpMarker : MonoBehaviour
{
    Image mpMarker;
    [SerializeField] private SkillType skillType;
    [SerializeField]
    private float mpCost;

    public void Init(SkillData data)
    {
        mpCost = data.mpCost;
    }

    private void Awake()
    {
        mpMarker = GetComponent<Image>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<MpChangeEvent>(OnChangeMp);
        mpMarker.enabled = false;
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<MpChangeEvent>(OnChangeMp);
    }

    private void OnChangeMp(MpChangeEvent changeEvent)
    {
        float curMp = changeEvent.CurrentMP;
        bool canUse = curMp >= mpCost;
        mpMarker.enabled = !canUse;
    }

}
