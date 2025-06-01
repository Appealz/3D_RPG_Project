using UnityEngine;

public struct indicatorEvent
{
    public IndicatorType IndicatorType;
    public Vector3 OriginPos;
    public float range;
    public indicatorEvent(IndicatorType indicatorType, Vector3 newOriginPos, float newRange)
    {
        IndicatorType = indicatorType;
        OriginPos = newOriginPos;
        range = newRange;
    }
}

public struct HideIndicatorEvent
{

}
