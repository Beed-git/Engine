using Microsoft.Extensions.Logging;

namespace Engine.Core.Config;

public class EngineConfig
{
    public required ILoggerFactory LoggerFactory { get; init; }
    public required ComponentConfig ComponentConfig { get; init; }
    public required FileSystemConfig FileSystemConfig { get; init; }
    public required FontConfig FontConfig { get; init; }
    public required PostInitializeConfig PostInitializeConfig{ get; init; }
    public required SerializationConfig SerializationConfig { get; init; }
    public required StageCollectionConfig StageCollectionConfig { get; init; }
}
