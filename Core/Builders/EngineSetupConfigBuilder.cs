using Engine.Core.Config;
using Engine.Core.Config.Internal;
using Microsoft.Extensions.Logging;

namespace Engine.Core.Builders;

public class EngineSetupConfigBuilder
{
    private readonly EngineSetupConfig _config;
    private ILoggerFactory? _loggerFactory;

    internal EngineSetupConfigBuilder()
    {
        _config = new EngineSetupConfig();
        _loggerFactory = null;
    }

    public EngineSetupConfigBuilder AddLogging(Action<ILoggingBuilder> configure)
    {
        _loggerFactory = LoggerFactory.Create(configure);
        return this;
    }

    public EngineSetupConfigBuilder AddComponents(Action<ComponentConfig> configure)
    {
        _config.ComponentConfig = configure;
        return this;
    }

    public EngineSetupConfigBuilder ConfigureSerialization(Action<SerializationConfig> configure)
    {
        _config.SerializationConfiguration = configure;
        return this;
    }

    public EngineSetupConfigBuilder ConfigureFileSystem(Action<FileSystemConfig> configure)
    {
        _config.FileSystemConfig = configure;
        return this;
    }

    public EngineSetupConfigBuilder ConfigureFonts(Action<FontConfig> configure)
    {
        _config.FontConfiguration = configure;
        return this;
    }

    public EngineInitialStageBuilder Initialize()
    {
        _loggerFactory ??= LoggerFactory.Create((config) => { });
        var builder = new EngineInitialStageBuilder(_loggerFactory, _config);
        return builder;
    }
}
