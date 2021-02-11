using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
[UpdateAfter(typeof(TeardownHybridPrefabSystem))]
public class InstantiateHybridPrefabSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithName("InstantiateHybridPrefab")
            .WithoutBurst()
            .WithStructuralChanges()
            .WithNone<LinkedHybridPrefabData>()
            .ForEach((Entity e, in LinkedHybridPrefabComponent prefabComponent) =>
            {
                var instance = HybridPrefabPool.GetInstance(prefabComponent.PrefabId);
#if UNITY_EDITOR
                instance.name = $"{prefabComponent.PrefabId} : {instance.GetInstanceID()} ({World.Name})";
#endif
                for (int i = 0; i < instance.Callbacks.Length; i++)
                {
                    instance.Callbacks[i].OnPoolInstantiate(e, World);
                }
                
                EntityManager.AddComponentData(e, new LinkedHybridPrefabData
                {
                    InstanceId = instance.GetInstanceID()
                });

                for (int i = 0; i < instance.ComponentsToAdd.Length; i++)
                {
                    EntityManager.AddComponentObject(e, instance.ComponentsToAdd[i]);
                }

                if (prefabComponent.EntityNeedsTransform)
                {
                    EntityManager.AddComponentObject(e, instance.transform);
                }
            }).Run();
    }
}