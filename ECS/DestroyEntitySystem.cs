using Arch.Core;
using Engine.ECS.Tags;
using Engine.Level;

namespace Engine.ECS;

public class DestroyEntitySystem
{
    private readonly QueryDescription _query;

    public DestroyEntitySystem()
    {
        _query = new QueryDescription()
            .WithAll<DestroyEntityTag>();
    }

    public void Update(World? entities)
    {
        entities?.Destroy(in _query);
    }
}
