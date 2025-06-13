using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{    
    private GameObject owner;

    [SerializeField]
    private PlayerBar bar;   
    [SerializeField]
    private HpGlobe hpGlobe;
    [SerializeField]
    private MpGlobe mpGlobe;

    [Header("Skill MP Markers")]
    [SerializeField] private SkillMpMarker qMarker;
    [SerializeField] private SkillMpMarker wMarker;
    [SerializeField] private SkillMpMarker eMarker;
    [SerializeField] private SkillMpMarker rMarker;

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
        bar.SetTarget(owner);
        hpGlobe.SetTarget(owner);
        //mpGlobe.SetTarget(owner);
    }

    public void SetSkillMpMarkers(SkillData qData, SkillData wData, SkillData eData, SkillData rData)
    {
        qMarker.Init(qData);
        wMarker.Init(wData);
        eMarker.Init(eData);
        rMarker.Init(rData);
    }
}
