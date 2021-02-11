using Unity.Entities;

[GenerateAuthoringComponent]
public struct DestroyInTime : IComponentData
{
    public float TimeToDestroy;
}

public class DestroyInTimeSystem : SystemBase
{
    private EndSimulationEntityCommandBufferSystem _cmdBufferSystem;

    protected override void OnCreate()
    {
        _cmdBufferSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var cmdBuffer = _cmdBufferSystem.CreateCommandBuffer();
        var deltaTime = Time.DeltaTime;
        
        Entities
            .ForEach((Entity e, ref DestroyInTime timer) =>
            {
                timer.TimeToDestroy -= deltaTime;

                if (timer.TimeToDestroy <= 0)
                {
                    cmdBuffer.DestroyEntity(e);
                }
            }).Run();
    }
}
