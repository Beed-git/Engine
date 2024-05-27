using Engine.Core.Config;
using Engine.Core.Config.Internal;
using Engine.Core.Internal;
using Microsoft.Extensions.Logging;

namespace Engine.Core.Builders;

public class EngineStageBuilder
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly EngineSetupConfig _config;
    private readonly StageBuilder _stages;

    internal EngineStageBuilder(ILoggerFactory loggerFactory, EngineSetupConfig config, StageBuilder stages)
    {
        _loggerFactory = loggerFactory;
        _config = config;
        _stages = stages;
    }

    public EngineStageBuilder AddAdditionalStage(string name, Action<StageConfig, Dependencies> configure)
    {
        _stages.Add(name, configure);
        return this;
    }

    public void Run()
    {
        var core = new EngineCore(_loggerFactory, _config, _stages);
        using var game = new MainGame(_loggerFactory, core);
        game.Run();
    }
}
