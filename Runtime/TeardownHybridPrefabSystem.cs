using Unity.Entities;

[UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
public class TeardownHybridPrefabSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem _cmdBufferSystem;

    protected override void OnCreate()
    {
        _cmdBufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var cmdBuffer = _cmdBufferSystem.CreateCommandBuffer();
        
        Entities
            .WithName("TeardownHybridPrefab")
            .WithoutBurst()
            .WithNone<LinkedHybridPrefabComponent>()
            .ForEach((Entity e, in LinkedHybridPrefabData instanceData) =>
            {
                HybridPrefabPool.ReturnInstance(instanceData.InstanceId);
                cmdBuffer.RemoveComponent<LinkedHybridPrefabData>(e);
            }).Run();
    }
}