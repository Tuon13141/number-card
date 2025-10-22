using System;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : Singleton<PoolingManager>
{
    [SerializeField] private PoolingConfig config;

    private readonly Dictionary<Type, Queue<MonoBehaviour>> poolDict = new();
    private Transform poolRoot;
    private const int PRELOAD_COUNT = 10;

    protected override void Awake()
    {
        base.Awake();

        if (config == null)
        {
            Debug.LogError("[PoolingManager] Chưa gán PoolingConfig!");
            return;
        }

        poolRoot = new GameObject("[PoolRoot]").transform;
        poolRoot.SetParent(transform);

        PreloadAll();
    }

    private void PreloadAll()
    {
        foreach (var prefab in config.prefabs)
        {
            if (prefab == null) continue;

            var type = prefab.GetType();
            if (!poolDict.ContainsKey(type))
                poolDict[type] = new Queue<MonoBehaviour>();

            for (int i = 0; i < PRELOAD_COUNT; i++)
            {
                var obj = Instantiate(prefab.gameObject, poolRoot);
                obj.SetActive(false);
                poolDict[type].Enqueue(obj.GetComponent<MonoBehaviour>());
            }
        }
    }

    public T Get<T>() where T : MonoBehaviour
    {
        var type = typeof(T);

        if (!poolDict.ContainsKey(type))
            poolDict[type] = new Queue<MonoBehaviour>();

        if (poolDict[type].Count == 0)
        {
            var prefab = FindPrefab<T>();
            if (prefab == null)
            {
                Debug.LogError($"[PoolingManager] Không tìm thấy prefab cho {type.Name}");
                return null;
            }

            var newObj = Instantiate(prefab.gameObject);
            newObj.name = $"{type.Name}_Pooled";
            return newObj.GetComponent<T>();
        }

        var instance = poolDict[type].Dequeue() as T;
        instance.gameObject.SetActive(true);
        return instance;
    }

    public void Release<T>(T obj) where T : MonoBehaviour
    {
        var type = typeof(T);

        if (!poolDict.ContainsKey(type))
            poolDict[type] = new Queue<MonoBehaviour>();

        obj.gameObject.SetActive(false);
        obj.transform.SetParent(poolRoot);
        poolDict[type].Enqueue(obj);
    }

    private T FindPrefab<T>() where T : MonoBehaviour
    {
        foreach (var prefab in config.prefabs)
        {
            if (prefab is T match)
                return match;
        }
        return null;
    }
}
