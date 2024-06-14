
using Engine.Resources;

namespace Engine.Files;

public class FileSystemSettings
{
    public FileSystemSettings(string root)
    {
        RootDirectory = root;
    }

    public string RootDirectory { get; private init; }

    // Constants.
    public static string AssetsFolder => $"assets{ResourceName.SeparatorChar}";
}
