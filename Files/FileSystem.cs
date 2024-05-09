using Engine.Resources;
using Engine.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Engine.Files;

public class FileSystem
{
    private readonly FileSystemSettings _settings;
    private readonly Serializer _serializer;

    private const string JsonExtension = ".json";

    internal FileSystem(FileSystemSettings settings, Serializer serializer)
    {
        _settings = settings;
        _serializer = serializer;
    }

    // Asset handling.

    public bool ExistsJsonAsset(Resource resource)
    {
        var file = CreateFilePath(resource);
        var path = Path.ChangeExtension(file, JsonExtension);
        var exists = File.Exists(path);
        return exists;
    }

    public bool TryReadJsonAsset<T>(Resource resource, [MaybeNullWhen(false)] out T asset)
    {
        var file = CreateFilePath(resource);
        var path = Path.ChangeExtension(file, JsonExtension);
        if (!File.Exists(path))
        {
            asset = default;
            return false;
        }

        var json = File.ReadAllText(path);
        asset = _serializer.Deserialize<T>(json)
            ?? throw new Exception($"Failed to read resource with name '{resource}' as json.");
        return true;
    }

    public void WriteJsonAsset<T>(Resource resource, T value)
    {
        var file = CreateFilePath(resource);
        var path = Path.ChangeExtension(file, JsonExtension);
        var json = _serializer.Serialize(value);
        File.WriteAllText(path, json);
    }

    public bool TryReadBinary(Resource resource, string extension, out byte[] asset)
    {
        var file = CreateFilePath(resource);
        var path = Path.ChangeExtension(file, extension);
        if (!File.Exists(path))
        {
            asset = [];
            return false;
        }

        asset = File.ReadAllBytes(path);
        return true;
    }

    public bool TryOpenBinaryStream(Resource resource, string extension, [MaybeNullWhen(false)] out Stream stream)
    {
        var file = CreateFilePath(resource);
        var path = Path.ChangeExtension(file, extension);
        if (!File.Exists(path))
        {
            stream = null;
            return false;
        }

        stream = File.OpenRead(path);
        return true;
    }

    private string CreateFilePath(Resource resource)
    {
        var split = resource.Id
            .ToLower()
            .Split(Resource.Separator);

        var output = Path.Combine(_settings.RootDirectory, Path.Combine(split));
        return output;
    }
}
