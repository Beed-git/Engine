using Engine.Resources;
using Engine.Serialization;
using System.Diagnostics.CodeAnalysis;

namespace Engine.Files;

public class FileSystem
{
    private readonly FileSystemSettings _settings;
    private readonly Serializer _serializer;

    internal FileSystem(FileSystemSettings settings, Serializer serializer)
    {
        _settings = settings;
        _serializer = serializer;
    }

    public bool Exists(ResourceName resource)
    {
        var path = CreateFilePath(resource);
        var exists = File.Exists(path);
        return exists;
    }

    [Obsolete($"Use {nameof(Serializer)} instead.")]
    public bool TryReadJsonAsset<T>(ResourceName resource, [MaybeNullWhen(false)] out T asset)
    {
        var path = CreateFilePath(resource);
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

    public byte[] ReadBinary(ResourceName name)
    {
        if (TryReadBinary(name, out var data))
        {
            return data;
        }

        throw new Exception($"Failed to find file '{name}'");
    }

    public bool TryReadBinary(ResourceName resource, out byte[] asset)
    {
        var path = CreateFilePath(resource);
        if (!File.Exists(path))
        {
            asset = [];
            return false;
        }

        asset = File.ReadAllBytes(path);
        return true;
    }

    public Stream OpenReadStream(ResourceName name)
    {
        var path = CreateFilePath(name);
        var stream = File.OpenRead(path);
        return stream;
    }

    public Stream OpenWriteStream(ResourceName name)
    {
        var path = CreateFilePath(name);
        var stream = File.OpenWrite(path);
        return stream;
    }

    public bool TryOpenStream(ResourceName resource, [MaybeNullWhen(false)] out Stream stream)
    {
        var path = CreateFilePath(resource);
        if (!File.Exists(path))
        {
            stream = null;
            return false;
        }

        stream = File.OpenRead(path);
        return true;
    }

    // Helpers.

    private string CreateFilePath(ResourceName resource)
    {
        var path = resource.Id;
        if (!Path.HasExtension(path))
        {
            path = Path.ChangeExtension(path, ResourceName.DefaultExtension);
        }

        // TODO: Validate path.

        var split = path
            .Split(ResourceName.SeparatorChar);

        var output = Path.Combine(_settings.RootDirectory, Path.Combine(split));
        return output;
    }
}
