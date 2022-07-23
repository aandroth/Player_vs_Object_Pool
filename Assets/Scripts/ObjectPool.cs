using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    delegate void deactivateCreepInList(int index);
    public static ObjectPool SharedInstance;
    public List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    public GameObject player;
    public GameObject boss;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        for(int ii=0; ii<amountToPool; ++ii)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            tmp.GetComponent<CreepScript>().objectPoolIndex = ii;
            tmp.GetComponent<CreepScript>().Player = player;
            pooledObjects.Add(tmp);
        }
    }

    public void deactivateCreepInListMethod(int index)
    {
        pooledObjects[index].gameObject.SetActive(false);
    }

    public GameObject GetPooledObject()
    {
        for (int ii = 0; ii < pooledObjects.Count; ++ii)
        {
            if (!pooledObjects[ii].activeSelf)
            {
                return pooledObjects[ii];
            }
        }
        return null;
    }

    public void ReleasePooledObjectAtIndex(int index)
    {
        pooledObjects[index].GetComponent<CreepScript>().ReleaseToObjectPool();
    }

    public CreepScript GetCreepScriptAtIndex(int index)
    {
        return pooledObjects[index].GetComponent<CreepScript>();
    }
}
