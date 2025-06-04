using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    List<Transform> spawnPoint = new List<Transform>();

    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        for(int i =0; i < spawnPoint.Count; i++)
        {
            int index = i;
            for(int j=0; j <  spawnPoint[i].childCount; j++)
            {
                int cur = Random.Range(0, 2);
                GameObject obj = ObjectPoolManager.Instance.pool[7 + cur].PopObj();
                obj.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent);
                agent.Warp(spawnPoint[index].GetChild(cur).position);
                //agent.enabled = false;
                //obj.transform.position = spawnPoint[index].GetChild(cur).position;
                //agent.enabled = true;
            }
        }
    }
}
