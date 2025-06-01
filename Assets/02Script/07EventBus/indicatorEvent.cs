using UnityEngine;

public struct indicatorEvent
{
    public IndicatorType IndicatorType;
    public Vector3 OriginPos;
    public float range;
    /// <summary>
    /// 인디케이터 타입, 위치, 범위
    /// </summary>
    /// <param name="indicatorType"></param>
    /// <param name="newOriginPos"></param>
    /// <param name="newRange"></param>
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
