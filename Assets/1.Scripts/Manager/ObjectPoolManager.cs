using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager <T> : MonoBehaviour where T : Component
{
    public T prefab;
    public int initialSize = 10;

    private Queue<T> pool = new Queue<T>();

    void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    T CreateNewObject()
    {
        T obj = Instantiate(prefab, transform);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    public T Dequeue()
    {
        if (pool.Count == 0)
        {
            CreateNewObject();
        }

        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Enqueue(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
