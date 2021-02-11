using UnityEngine;

public class HybridPrefabCollection : MonoBehaviour
{
    public HybridPrefab[] Prefabs;

    private void Awake()
    {
        foreach (var prefab in Prefabs)
        {
            HybridPrefabPool.RegisterPrefab(prefab);
        }
    }
}
