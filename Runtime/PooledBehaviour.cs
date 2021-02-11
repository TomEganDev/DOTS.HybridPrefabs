using Unity.Entities;
using UnityEngine;

public abstract class PooledBehaviour : MonoBehaviour, IOnPoolInstantiate
{
    public abstract void OnPoolInstantiate(Entity entity, World world);
}
