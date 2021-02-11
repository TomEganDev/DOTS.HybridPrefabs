using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class TestSpawner : MonoBehaviour, IConvertGameObjectToEntity, IDeclareReferencedPrefabs
{
    public GameObject Prefab;
    public float TimePerSpawn;
    public bool InstantSpawn = true;
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new TestSpawnerData
        {
            Prefab = conversionSystem.GetPrimaryEntity(Prefab),
            SpawnCooldown = TimePerSpawn,
            SpawnTimer = InstantSpawn ? 0 : TimePerSpawn
        });
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(Prefab);
    }
}

public struct TestSpawnerData : IComponentData
{
    public Entity Prefab;
    public float SpawnCooldown;
    public float SpawnTimer;
}

[UpdateInGroup(typeof(InitializationSystemGroup))]
public class TestSpawnerSystem : SystemBase
{
    private EndInitializationEntityCommandBufferSystem _cmdBufferSystem;

    protected override void OnCreate()
    {
        _cmdBufferSystem = World.GetExistingSystem<EndInitializationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        var cmdBuffer = _cmdBufferSystem.CreateCommandBuffer();
        var deltaTime = Time.DeltaTime;
        
        Entities
            .ForEach((ref TestSpawnerData spawner) =>
            {
                spawner.SpawnTimer -= deltaTime;
                if (spawner.SpawnTimer <= 0)
                {
                    spawner.SpawnTimer += spawner.SpawnCooldown;
                    cmdBuffer.Instantiate(spawner.Prefab);
                }
            }).Run();
    }
}