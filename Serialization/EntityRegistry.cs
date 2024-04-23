using Arch.Core;
using Engine.ECS.Components;

namespace Engine.Serialization;

public class EntityRegistry
{
    private readonly QueryDescription _query;
    private readonly QueryDescription _noNameQuery;

    private readonly Dictionary<string, Entity> _namedEntities;
    private World? _current;

    public EntityRegistry()
    {
        _query = new QueryDescription()
            .WithAll<NameComponent>();

        _noNameQuery = new QueryDescription()
            .WithNone<NameComponent>();

        _namedEntities = [];
        _current = null;
    }

    public World? Current
    {
        get => _current;
        set
        {
            _namedEntities.Clear();
            _current = value;
            _current?.Query(in _query, (Entity entity, ref NameComponent name) =>
            {
                _namedEntities.Add(name.Name, entity);
            });
        }
    }

    public Entity GetOrCreate(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        ThrowIfWorldInvalid();

        if (!_namedEntities.TryGetValue(name, out var entity))
        {
            entity = Current!.Create();
            _namedEntities.Add(name, entity);
        }

        return entity;
    }

    private void ThrowIfWorldInvalid()
    {
        if (Current is null)
        {
            throw new Exception($"{nameof(Current)} must not be null.");
        }
    }
}
