using Engine.Resources;
using System.Diagnostics.CodeAnalysis;

namespace Engine.Level;

public class Scene
{
    private readonly Dictionary<Type, object> _objects;

    internal Scene(ResourceName name)
    {
        Name = name;
        _objects = [];
    }

    public ResourceName Name { get; private init; }

    public T? Get<T>()
        where T : class
    {
        TryGet<T>(out var result);
        return result;
    }
    
    public bool TryGet<T>([MaybeNullWhen(false)] out T value)
        where T : class
    {
        var type = typeof(T);
        if (!_objects.TryGetValue(type, out var obj))
        {
            value = default;
            return false;
        }

        if (obj is not T t)
        {
            throw new Exception($"Object type '{obj.GetType()}' does not match expected type '{type}' ");
        }

        value = t;
        return true;
    }
    
    public bool Has<T>() 
        where T : class
    {
        return _objects.ContainsKey(typeof(T));
    }

    public void Add<T>(T obj)
        where T : class
    {
        _objects.Add(typeof(T), obj);
    }

    public bool Remove<T>()
    {
        return _objects.Remove(typeof(T));
    }
}