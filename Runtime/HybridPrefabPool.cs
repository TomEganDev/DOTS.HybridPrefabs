using System.Collections.Generic;
using UnityEngine;

public static class HybridPrefabPool
{
    private static Dictionary<ushort, HybridPrefab> _prefabLookup = new Dictionary<ushort, HybridPrefab>();
    private static Dictionary<ushort, Queue<HybridPrefab>> _poolLookup = new Dictionary<ushort, Queue<HybridPrefab>>();
    private static Dictionary<int, HybridPrefab> _activeInstanceLookup = new Dictionary<int, HybridPrefab>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        _prefabLookup.Clear();
        _poolLookup.Clear();
        _activeInstanceLookup.Clear();
    }

    public static void RegisterPrefab(HybridPrefab prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Cannot register null prefab");
            return;
        }
        
        if (_prefabLookup.ContainsKey(prefab.Id))
        {
            var registered = _prefabLookup[prefab.Id];
            if (registered == prefab)
            {
                Debug.LogWarning($"{prefab.Id}:{prefab.name} dual registration ignored", prefab);
            }
            else
            {
                Debug.LogError("HybridPrefab.Id collision!");
                Debug.LogError($"{registered.name} - registered", registered);
                Debug.LogError($"{prefab.name} - conflict", prefab);
            }

            return;
        }
        
        _prefabLookup.Add(prefab.Id, prefab);
        // pre-allocate pool
        var pool = new Queue<HybridPrefab>(prefab.PrePoolSize);
        _poolLookup.Add(prefab.Id, pool);
        for (int i = 0; i < prefab.PrePoolSize; i++)
        {
            var instance = Object.Instantiate(prefab);
            
#if UNITY_EDITOR
            instance.gameObject.hideFlags = HideFlags.HideInHierarchy;
#endif
            
            instance.gameObject.SetActive(false);
            pool.Enqueue(instance);
        }
    }
    
    public static HybridPrefab GetInstance(ushort prefabId)
    {
        if (!_prefabLookup.ContainsKey(prefabId))
        {
            Debug.LogError($"Couldn't find prefab {prefabId}");
            return null;
        }

        var pool = _poolLookup[prefabId];
        HybridPrefab instance;
        if (pool.Count > 0)
        {
            instance = pool.Dequeue();
            instance.gameObject.SetActive(true);
        }
        else
        {
            var prefab = _prefabLookup[prefabId];
            instance = Object.Instantiate(prefab);
        }

#if UNITY_EDITOR
        instance.gameObject.hideFlags = HideFlags.None;
#endif
        
        _activeInstanceLookup.Add(instance.GetInstanceID(), instance);

        return instance;
    }

    public static bool TryGetActiveInstance(int instanceId, out HybridPrefab instance)
    {
        return _activeInstanceLookup.TryGetValue(instanceId, out instance);
    }

    public static void ReturnInstance(int instanceId)
    {
        if (!TryGetActiveInstance(instanceId, out var instance))
        {
            Debug.LogError($"instance {instanceId} was not in _activeInstanceLookup, could not return");
            return;
        }

        _activeInstanceLookup.Remove(instanceId);
        var prefabId = instance.Id;
        var pool = _poolLookup[prefabId];
        instance.gameObject.SetActive(false);
#if UNITY_EDITOR
        instance.gameObject.hideFlags = HideFlags.HideInHierarchy;
#endif
        pool.Enqueue(instance);
    }
}
