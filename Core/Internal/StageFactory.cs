using Engine.Core.Config;
using Engine.ECS;
using Engine.Level;
using Microsoft.Extensions.Logging;

namespace Engine.Core.Internal;

internal class StageFactory
{
    private readonly ILogger<StageFactory> _logger;
    private readonly StaticServices _statics;
    private readonly Dictionary<string, Action<StageConfig, Services>> _stages;

    internal StageFactory(
        ILoggerFactory loggerFactory,
        StaticServices dependencies,
        IEnumerable<KeyValuePair<string, Action<StageConfig, Services>>> stages)
    {
        _logger = loggerFactory.CreateLogger<StageFactory>();
        _statics = dependencies;
        _stages = stages.ToDictionary(); 
    }

    public Stage Create(string name)
    {
        if (!_stages.TryGetValue(name, out var configure))
        {
            throw new Exception($"Stage with name '{name}' not found.");
        }

        _logger.LogInformation("Creating stage '{}'", name);

        // Set dependencies.
        var sceneManager = new SceneManager(_statics.LoggerFactory, _statics.FileSystem);
        var eventRegistry = new EventRegistry(_statics.LoggerFactory);
        var events = new EventSystem(eventRegistry);

        var services = new Services(_statics, sceneManager, events);
        
        // Register.
        var config = new StageConfig();
        configure.Invoke(config, services);

        foreach (var register in config.EventRegisterSystems)
        {
            register.Value.Invoke(eventRegistry);
        }

        var stage = new Stage(
            name,
            sceneManager,
            eventRegistry,
            events,
            config.UpdateSystems,
            config.RenderSystems,
            config.DebugUIs);

        return stage;
    }
}
