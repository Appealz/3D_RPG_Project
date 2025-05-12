using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<T>();

                if(instance == null)
                {
                    var singletonObj = new GameObject(typeof(T).Name);
                    instance = singletonObj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DoAwake();
    }

    protected virtual void DoAwake()
    {
        
    }
}

public class DontDestroySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();

                if (instance == null)
                {
                    var singletonObj = new GameObject(typeof(T).Name);
                    instance = singletonObj.AddComponent<T>();
                    DontDestroyOnLoad(singletonObj);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DoAwake();
    }

    protected virtual void DoAwake()
    {

    }
}