using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField]
    GameObject owner;
    Image hpBar;

    private void Awake()
    {
        hpBar = GetComponent<Image>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<HpChangeEvent>(OnChangeHPEvent);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<HpChangeEvent>(OnChangeHPEvent);
    }

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }

    public void OnChangeHPEvent(HpChangeEvent hpChangeEvent)
    {
        if(hpChangeEvent.Publisher != owner)
        {
            return;
        }

        float ratio = hpChangeEvent.CurrentHp / hpChangeEvent.MaxHp;

        hpBar.fillAmount = ratio;
    }
}
