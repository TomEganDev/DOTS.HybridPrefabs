using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class LinkHybridPrefab : MonoBehaviour, IConvertGameObjectToEntity
{
    public HybridPrefab Prefab;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        if (Prefab == null)
        {
            Debug.LogError($"{gameObject.name}.LinkHybridPrefab.Prefab was null during conversion!", this);
            return;
        }

        dstManager.AddComponentData(entity, new LinkedHybridPrefabComponent
        {
            PrefabId = Prefab.Id,
            EntityNeedsTransform = Prefab.CopyTransformToPrefab || Prefab.CopyTransformFromPrefab
        });

        if (Prefab.CopyTransformFromPrefab)
        {
            dstManager.AddComponent<CopyTransformFromGameObject>(entity);
        }

        if (Prefab.CopyTransformToPrefab)
        {
            dstManager.AddComponent<CopyTransformToGameObject>(entity);
        }
    }
}

public struct LinkedHybridPrefabComponent : IComponentData
{
    public ushort PrefabId;
    public bool EntityNeedsTransform;
}

public struct LinkedHybridPrefabData : ISystemStateComponentData
{
    public int InstanceId;
}
