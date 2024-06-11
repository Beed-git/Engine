namespace Engine.Resources;

public class Resource<T>
    where T : class
{
    public Resource(ResourceName name, T data)
    {
        ArgumentNullException.ThrowIfNull(data, nameof(data));

        Data = data;
        Name = name;
    }

    public ResourceName Name { get; private init; }
    public T Data { get; private init; }

    public static implicit operator T(Resource<T> resource) => resource.Data;
    public static implicit operator ResourceName(Resource<T> resource) => resource.Name;
}
