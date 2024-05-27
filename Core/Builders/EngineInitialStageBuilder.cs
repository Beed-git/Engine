using Engine.Core.Config;
using Engine.Core.Config.Internal;
using Microsoft.Extensions.Logging;

namespace Engine.Core.Builders;

public class EngineInitialStageBuilder
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly EngineSetupConfig _config;

    internal EngineInitialStageBuilder(ILoggerFactory loggerFactory, EngineSetupConfig config)
    {
        _loggerFactory = loggerFactory;
        _config = config;
    }

    public EngineStageBuilder SetInitialStage(string name, Action<StageConfig, Dependencies> configure)
    {
        var stages = new StageBuilder(name, configure);
        var builder = new EngineStageBuilder(_loggerFactory, _config, stages);
        return builder;
    }
}
