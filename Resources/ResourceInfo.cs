namespace Engine.Resources;

public class ResourceInfo<T>
{
    private readonly Dictionary<ResourceName, T> _internalCache;
    private readonly Func<ResourceName, T>? _jsonLoaderFunction;
    private readonly IRawResourceLoader<T>? _rawLoader;

    private readonly T? _defaultValue;

    internal ResourceInfo(Func<ResourceName, T>? jsonLoaderFunction, IRawResourceLoader<T>? rawResourceLoader, T? defaultValue = default)
    {
        _internalCache = [];
        _rawLoader = rawResourceLoader;
        _jsonLoaderFunction = jsonLoaderFunction;
        _defaultValue = defaultValue;
    }

    public T? Get(ResourceName name)
    {
        if (name.IsInternalResource)
        {
            if (!_internalCache.TryGetValue(name, out var resource))
            {
                // TODO: log.
                resource = _defaultValue;
            }

            return resource;
        }
        else if (name.HasExtension)
        {
            if (_rawLoader is null)
            {
                // TODO: log.
                return _defaultValue;
            }
            
            var resource = _rawLoader.Load(name);
            return resource;
        }
        else
        {
            if (_jsonLoaderFunction is null)
            {
                // TODO: log.
                return _defaultValue;
            }

            var resource = _jsonLoaderFunction.Invoke(name);
            return resource;
        }
    }

    public void AddToCache(ResourceName name, T value)
    {
        // TODO: Do we need this?
        if (!name.IsInternalResource)
        {
            throw new Exception("Cannot add to cache if not internal resource.");
        }

        _internalCache.Add(name, value);
    }
}
