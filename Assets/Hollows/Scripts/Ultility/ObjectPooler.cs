using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] protected GameObject objectPrefab;
    [SerializeField] protected int poolSize;
    protected List<GameObject> pooler = new List<GameObject>();

    protected virtual void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab, transform);
            pooler.Add(obj);
            obj.SetActive(false);
        }
    }

    public GameObject GetObject()   // Set active = true when get object
    {
        foreach (var obj in pooler)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }
        GameObject newObj = Instantiate(objectPrefab, transform);
        pooler.Add(newObj);
        return newObj;
    }

    public virtual void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
}
