using Engine.ECS;
using Engine.Level;
using Microsoft.Extensions.Logging;

namespace Engine.Configuration.Internal;

internal class StageRepository
{
    private readonly ILogger<StageRepository> _logger;
    private readonly Dependencies _dependencies;
    private readonly Dictionary<string, Action<StageConfig, Dependencies>> _stages;

    internal StageRepository(
        ILoggerFactory loggerFactory,
        Dependencies dependencies,
        IEnumerable<KeyValuePair<string, Action<StageConfig, Dependencies>>> stages)
    {
        _logger = loggerFactory.CreateLogger<StageRepository>();
        _dependencies = dependencies;
        _stages = stages.ToDictionary(); 
    }

    public Stage Create(string name)
    {
        if (!_stages.TryGetValue(name, out var configure))
        {
            throw new Exception($"Stage with name '{name}' not found.");
        }

        _logger.LogInformation("Creating stage '{}'", name);
        var sceneManager = new SceneManager(_dependencies.LoggerFactory, _dependencies.FileSystem);

        var events = new EventRegistry(_dependencies.LoggerFactory);
        
        _dependencies.SceneManager = sceneManager;
        _dependencies.Events = new EventSystem(events);

        var config = new StageConfig(name);
        configure.Invoke(config, _dependencies);

        foreach (var register in config.EventRegisterSystems)
        {
            register.Value.Invoke(events);
        }

        var stage = new Stage(
            name,
            sceneManager,
            events,
            config.UpdateSystems,
            config.RenderSystems,
            config.DebugUIs);

        return stage;
    }
}
