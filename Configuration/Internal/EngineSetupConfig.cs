using Engine.ECS;
using Engine.Serialization;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Engine.Configuration.Internal;

internal class EngineSetupConfig
{
    internal Action<ComponentConfig>? ComponentConfig { get; set; }
    internal Action<FileSystemConfig>? FileSystemConfig { get; set; }
    internal Action<FontConfig>? FontConfiguration { get; set; }
    internal Action<SerializationConfig>? SerializationConfiguration { get; set; }
}
