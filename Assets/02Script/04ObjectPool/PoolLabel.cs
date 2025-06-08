using UnityEngine;

public class PoolLabel : MonoBehaviour
{
    protected ObjectPool myPool;

    public void Create(ObjectPool newPool)
    {
        myPool = newPool;
        gameObject.SetActive(false);
    }

    public void ReturnPool()
    {
        if (!Application.isPlaying) return;

        myPool.PushObj(this);
    }
}
