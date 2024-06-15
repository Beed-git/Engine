using Engine.Resources;
using Engine.Serialization;
using System.Text.Json;

namespace Engine.Core.Config;

internal delegate T FileLoaderDelegate<T>(ResourceName resource, ref Utf8JsonReader reader, Serializer serializer);

public class ResourceConfig<T>
{
    public ResourceConfig()
    {
        TypeKey = typeof(T).Name;
        FallbackResource = default;
        InternalValues = [];
        RawLoader = null;
        FileLoaderFunction = null;
    }

    public string TypeKey { get; set; }
    public T? FallbackResource { get; set; }
    public List<KeyValuePair<ResourceName, T>> InternalValues { get; init; }
    public IRawResourceLoader<T>? RawLoader { get; set; }
    internal FileLoaderDelegate<T>? FileLoaderFunction { get; private set; }
    
    public void SetFileLoader<TModel>(IModelResourceLoader<T, TModel> loader)
    {
        FileLoaderFunction = LoadAssetFromFile;

        T LoadAssetFromFile(ResourceName resource, ref Utf8JsonReader reader, Serializer serializer)
        {
            var model = serializer.Deserialize<TModel>(ref reader);
            var instance = loader.Load(model);
            return instance;
        }
    }
}
