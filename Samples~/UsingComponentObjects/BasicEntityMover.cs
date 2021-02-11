using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[GenerateAuthoringComponent]
public struct BasicEntityMover : IComponentData {}

public class BasicEntityMoverSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var elapsedTime = UnityEngine.Time.time;
        
        Entities
            .WithAll<BasicEntityMover>()
            .ForEach((ref Translation translation) =>
            {
                translation.Value = new float3(math.sin(elapsedTime), 0, 0);
            }).Run();
    }
}