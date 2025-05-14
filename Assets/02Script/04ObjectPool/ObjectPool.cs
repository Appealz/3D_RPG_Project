using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabs;
    private GameObject obj;
    private PoolLabel objLabel;
    private Stack<PoolLabel> poolStack = new Stack<PoolLabel>();

    private int poolCount = 10;

    private void Awake()
    {
        Allocate();
    }

    private void Allocate()
    {
        for(int i = 0; i < poolCount; i++)
        {
            obj = Instantiate(prefabs, transform);            
            if(obj.TryGetComponent<PoolLabel>(out objLabel))
            {
                poolStack.Push(objLabel);
                objLabel.Create(this);
            }
        }
    }

    public GameObject PopObj()
    {
        if(poolStack.Count < 1)
        {
            Allocate();
        }
        objLabel = poolStack.Pop();
        objLabel.gameObject.SetActive(true);

        return objLabel.gameObject;
    }

    public void PushObj(PoolLabel returnObj)
    {
        returnObj.gameObject.SetActive(false);
        poolStack.Push(returnObj);
    }
}
