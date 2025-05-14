using UnityEngine;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [SerializeField]
    public ObjectPool pool;
}
