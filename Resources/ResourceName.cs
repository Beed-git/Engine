
namespace Engine.Resources;

public readonly partial struct ResourceName
{
    public const char Separator = '/';

    public readonly string Id;

    public ResourceName(string name)
    {
        Id = Normalize(name);
    }

    public readonly bool IsTopLevel()
    {
        return Id.Contains(Separator);
    }

    public readonly string GetDirectory()
    {
        var index = Id.LastIndexOf(Separator);
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
        var dirs = dir.Split(Separator);
        return dirs;
    }

    public readonly string GetFile()
    {
        var index = Id.IndexOf(Separator);
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
