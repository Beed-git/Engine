using Engine.Serialization;
using Engine.Files;
using System.Reflection;

namespace Engine.Core.Config.Factories;

internal static class FileSystemFactory
{
    internal static FileSystem Create(FileSystemConfig config, Serializer serializer)
    {
        var root = config.Root
            ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? throw new Exception("Root asset directory was not set and reflection fallback failed.");

        var fileSystemSettings = new FileSystemSettings(root);

        var fileSystem = new FileSystem(fileSystemSettings, serializer);
        return fileSystem;
    }
}
