using Engine.Core.Config;
using Engine.Files;
using Engine.Serialization;
using System.Text.Json;

namespace Engine.Resources;

// TODO: Add some sort of caching.
public class ResourceSystem
{
    private readonly FileSystem _files;
    private readonly Serializer _serializer;

    private readonly Dictionary<Type, object> _infos;

    internal ResourceSystem(FileSystem files, Serializer serializer)
    {
        _files = files;
        _serializer = serializer;

        _infos = [];
    }

    internal void AddConfig<T>(ResourceConfig<T> config)
    {
        Func<ResourceName, T>? jsonLoaderFunction = config.FileLoaderFunction is not null
            ? JsonLoadAction
            : null;
        var rawLoader = config.RawLoader;
        // TODO: Set default value.

        var info = new ResourceInfo<T>(jsonLoaderFunction, rawLoader);

        foreach (var (name, value) in config.InternalValues)
        {
            info.AddToCache(name, value);
        }

        _infos.Add(typeof(T), info);

        T JsonLoadAction(ResourceName name)
        {
            var bytes = _files.ReadBinary($"{FileSystemSettings.AssetsFolder}{name}");
            ReadMetadata(bytes);

            var reader = Search<T>(bytes, config.TypeKey);
            var result = config.FileLoaderFunction.Invoke(name, ref reader, _serializer);
            return result;
        }
    }

    public T? Get<T>(ResourceName name)
        where T : class
    {
        var type = typeof(T);
        if (!_infos.TryGetValue(type, out var info))
        {
            throw new Exception($"Type '{type}' is not a valid resource type.");
        }

        if (info is not ResourceInfo<T> resourceInfo)
        {
            throw new InvalidOperationException();
        }

        return resourceInfo.Get(name);
    }
    
    private ResourceMetadata ReadMetadata(ReadOnlySpan<byte> json)
    {
        var metadata = _serializer.Deserialize<ResourceMetadata>(json);
        if (metadata.FileVersion != ResourceMetadata.CurrentVersion)
        {
            throw new Exception($"Metadata version is out of date. The current supported version is {ResourceMetadata.CurrentVersion} but file version is {metadata.FileVersion}");
        }

        return metadata;
    }

    private Utf8JsonReader Search<T>(ReadOnlySpan<byte> json, string key)
    {
        var reader = _serializer.CreateReader(json);

        reader.Read();
        reader.Expect(JsonTokenType.StartObject);

        while (reader.Read())
        {
            if (reader.Is(JsonTokenType.EndObject))
            {
                throw new Exception($"Failed to find key '{key}' in metadata.");
            }

            reader.Expect(JsonTokenType.PropertyName);
            var name = reader.GetNonEmptyString();

            if (key.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                break;
            }
            else
            {
                reader.Skip();
            }
        }

        return reader;
    }
}