using System;
using System.Collections.Generic;
using UnityEngine;

public enum IndicatorType
{
    Circle,
    Fan,
    Area
}



public class IndicatorManager : MonoBehaviour
{
    private Dictionary<IndicatorType, IIndicator> indicators;
    
    private void Awake()
    {
        indicators = new Dictionary<IndicatorType, IIndicator>();
        indicators[IndicatorType.Circle] = GetComponentInChildren<CircleIndicator>();
        indicators[IndicatorType.Fan] = GetComponentInChildren<FanIndicator>();
        indicators[IndicatorType.Area] = GetComponentInChildren<SpellIndicator>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<indicatorEvent>(ShowIndiCatorEvent);
        EventBus.Subscribe<HideIndicatorEvent>(OnHideIndicator);
        PlayerSkillManager.indicatorOff += HideAllIndicator;
        HideAllIndicator();
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<indicatorEvent>(ShowIndiCatorEvent);
        EventBus.UnSubscribe<HideIndicatorEvent>(OnHideIndicator);
        PlayerSkillManager.indicatorOff -= HideAllIndicator;
    }

    public void ShowIndiCatorEvent(indicatorEvent indicator)
    {
        if(indicator.OriginPos == Vector3.zero)
        {
            indicator.OriginPos = transform.position;
        }
        indicators[indicator.IndicatorType].Show(indicator.OriginPos, Vector3.zero, indicator.range);
    }

    private void OnHideIndicator(HideIndicatorEvent a)
    {
        HideAllIndicator();
    }


    public void HideAllIndicator()
    {
        foreach(var indicator in indicators.Values)
        {
            indicator.Hide();
        }
    }
}
