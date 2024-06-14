
namespace Engine.Resources;

/// <summary>
/// The name of a resource
/// A name without an extension (e.g. level/map-1) will use the default resource 
/// extension (.json) and load using a ModelLoader.
/// A name with an extension will load directly from the binary and use the RawLoader.
/// A name which starts with '#' won't load from the disk.
/// </summary>
public readonly partial struct ResourceName
{
    internal const char SeparatorChar = '/';
    internal const char InternalResourceChar = '#';
    internal const string DefaultExtension = "json";

    public readonly string Id;

    public ResourceName(string name)
    {
        Id = Create(name);
    }

    public bool IsInternalResource => Id.StartsWith(InternalResourceChar);
    public bool HasExtension => Id.Contains('.');

    public readonly string GetDirectory()
    {
        var index = Id.LastIndexOf(SeparatorChar);
        if (index < 0)
        {
            return string.Empty;
        }

        var folder = Id[..index];
        return folder;
    }

    public readonly string[] GetFolders()
    {
        var index = Id.LastIndexOf('/');
        if (index < 0)
        {
            return [];
        }

        var dir = Id[..index];
        var dirs = dir.Split(SeparatorChar);
        return dirs;
    }

    public readonly string GetFileName()
    {
        var index = Id.LastIndexOf(SeparatorChar);
        if (index < 0)
        {
            return Id;
        }

        var file = Id[index..];
        return file;
    }

    public readonly override bool Equals(object? obj)
    {
        return obj is ResourceName resource &&
               Id == resource.Id;
    }

    public readonly override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public readonly override string ToString()
    {
        return Id.ToString();
    }

    public static implicit operator ResourceName(string resource) => new(resource);
    public static explicit operator string(ResourceName resource) => resource.Id;
    public static bool operator ==(ResourceName left, ResourceName right) => left.Equals(right);
    public static bool operator !=(ResourceName left, ResourceName right) => !(left == right);
}
