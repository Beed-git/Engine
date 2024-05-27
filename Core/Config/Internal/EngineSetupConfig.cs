namespace Engine.Core.Config.Internal;

internal class EngineSetupConfig
{
    internal Action<ComponentConfig>? ComponentConfig { get; set; }
    internal Action<FileSystemConfig>? FileSystemConfig { get; set; }
    internal Action<FontConfig>? FontConfiguration { get; set; }
    internal Action<SerializationConfig>? SerializationConfiguration { get; set; }
}
