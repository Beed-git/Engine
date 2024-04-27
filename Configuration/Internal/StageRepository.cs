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
        _dependencies.SceneManager = sceneManager;

        var config = new StageConfig(name);
        configure.Invoke(config, _dependencies);

        var stage = new Stage(
            name,
            sceneManager,
            config.InitSystems,
            config.DestroySystems,
            config.OnSceneLoadSystems,
            config.OnSceneUnloadSystems,
            config.UpdateSystems,
            config.RenderSystems,
            config.DebugUIs);

        return stage;
    }
}
