using Unity.VisualScripting;
using UnityEngine;

public class FindObjectTransform : MonoBehaviour
{
    /// <summary>
    /// �ڽ� ������Ʈ �� �̸��� ��ġ�ϴ� ������Ʈ�� ã�� Transform�� ��ȯ.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="childName"></param>
    /// <returns></returns>
    public static Transform FindChildTransform(Transform parent, string childName)
    {
        foreach(Transform child in parent)
        {
            if(child.name == childName)
            {
                return child;
            }
            Transform findRescursiveChild = FindChildTransform(child, childName);
            if(findRescursiveChild != null)
            {
                return findRescursiveChild;
            }
        }
        return null;
    }
}
