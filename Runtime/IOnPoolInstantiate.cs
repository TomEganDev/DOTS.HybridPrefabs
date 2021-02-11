using Unity.Entities;

public interface IOnPoolInstantiate
{
    public void OnPoolInstantiate(Entity entity, World world);
}