using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPool : MonoBehaviour
{

    public static BallPool SharedInstance;
    List<GameObject> pooledObjects;
    public GameObject[] ObjectsToPool;
    public int amountToPool;
    
    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject obj;
        for(int i=0; i < ObjectsToPool.Length; i++)
        {
            for(int j = 0;j < amountToPool; j++)
            {
                obj = Instantiate(ObjectsToPool[i]);
                obj.SetActive(false);
                obj.transform.SetParent(this.transform);
                pooledObjects.Add(obj);
            }
        }
    }

    public List<GameObject> PooledObjects()
    {
        List<GameObject> inactivePoolObjects = new List<GameObject>();
        foreach(var obj in pooledObjects)
        {
            if(!obj.activeInHierarchy)
                inactivePoolObjects.Add(obj);
        }

        return inactivePoolObjects;
    }
}
