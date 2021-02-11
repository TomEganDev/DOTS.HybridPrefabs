using UnityEngine;

public class HybridPrefab : MonoBehaviour
{
    public int PrePoolSize;
    public ushort Id;
    public bool CopyTransformToPrefab;
    public bool CopyTransformFromPrefab;
    public Component[] ComponentsToAdd;
    public PooledBehaviour[] Callbacks;

    [ContextMenu("Gather Instantiate Callbacks")]
    public void GatherInstantiateCallbacks()
    {
        Callbacks = GetComponentsInChildren<PooledBehaviour>();
    }
}