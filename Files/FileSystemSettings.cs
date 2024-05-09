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
    public static string AssetsFolder => $"assets{Resource.Separator}";
    public static string DataFolder => $"{AssetsFolder}data{Resource.Separator}";
    public static string FontsFolder => $"{AssetsFolder}fonts{Resource.Separator}";
    public static string ScenesFolder => $"{AssetsFolder}scenes{Resource.Separator}";
    public static string TexturesFolder => $"{AssetsFolder}textures{Resource.Separator}";
}
