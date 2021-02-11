using Unity.Entities;
using Unity.Transforms;
using UnityEngine.UI;

[GenerateAuthoringComponent]
public struct SetWorldUI : IComponentData {}

public class SetWorldUITextSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities
            .WithAll<SetWorldUI>()
            .WithoutBurst()
            .ForEach((Text label, in LocalToWorld ltw) =>
            {
                label.text = $"X = {ltw.Position.x:N2}";
            }).Run();
    }
}