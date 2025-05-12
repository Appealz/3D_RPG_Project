using Unity.VisualScripting;
using UnityEngine;

public class FindObjectTransform : MonoBehaviour
{
    /// <summary>
    /// 자식 오브젝트 중 이름이 일치하는 오브젝트를 찾아 Transform을 반환.
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
