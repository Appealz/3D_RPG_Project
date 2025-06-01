using UnityEngine;

public interface IIndicator
{
    void Show(Vector3 origin, Vector3 direction, float range, float angle = 0f);
    void Hide();
}
