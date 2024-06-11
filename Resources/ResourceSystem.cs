using Engine.Files;
using Engine.Serialization;
using System.Text.Json;

namespace Engine.Resources;

public class ResourceSystem
{
    private readonly FileSystem _files;
    private readonly Serializer _serializer;

    private readonly Dictionary<Type, object> _loaders;

    internal ResourceSystem(FileSystem files, Serializer serializer)
    {
        _files = files;
        _serializer = serializer;

        _loaders = [];
    }

    public void AddResourceLoader<T>(IResourceLoader<T> loader)
    {
        var key = typeof(T).Name;

        T LoadAsset(ResourceName resource)
        {
            var path = $"{FileSystemSettings.AssetsFolder}{resource}";
            var file = _files.ReadBinary(path, "json");

            var metadata = _serializer.Deserialize<ResourceMetadata>(file);
            if (metadata.FileVersion != ResourceMetadata.CurrentVersion)
            {
                throw new Exception($"Metadata version is out of date. The current supported version is {ResourceMetadata.CurrentVersion} but file version is {metadata.FileVersion}");
            }

            var reader = _serializer.CreateReader(file);

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

            var instance = loader.Load(ref reader);
            return instance;
        }

        _loaders.Add(typeof(T), (object)LoadAsset);
    }

    public T Get<T>(ResourceName resource)
        where T : class
    {
        if (!_loaders.TryGetValue(typeof(T), out var function))
        {
            throw new Exception();
        }

        if (function is not Func<ResourceName, T> loader)
        {
            throw new Exception();
        }

        var result = loader.Invoke(resource);
        return result;
    }
}