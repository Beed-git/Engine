using Engine.ECS;
using Engine.ECS.Events;
using Engine.Level;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;

namespace Engine.Configuration.Internal;

internal class SystemManager
{
    private readonly ILogger _logger;
    private readonly StageRepository _repository;
    private readonly StageManager _stages;
    private readonly SystemProfiler _systemProfiler;

    internal SystemManager(ILoggerFactory loggerFactory, StageRepository repository, StageManager stages)
    {
        _logger = loggerFactory.CreateLogger<SystemManager>();
        _repository = repository;
        _stages = stages;
        _systemProfiler = new SystemProfiler();
    }

    public void StageChange()
    {
        if (_stages.Next is null)
        {
            return;
        }

        var stage = _stages.CurrentStage;

        _logger.LogInformation("Destroying stage '{}'", stage.Name);
        stage.EventRegistry.Invoke<StageDestructEvent>(stage.SceneManager.Current);

        stage = _repository.Create(_stages.Next);
        _stages.CurrentStage = stage;
        _stages.Next = null;

        _logger.LogInformation("Initializing stage '{}'", stage.Name);
        stage.EventRegistry.Invoke<StageInitialiseEvent>(stage.SceneManager.Current);
    }

    public void SceneChange()
    {
        var stage = _stages.CurrentStage;
        var scenes = stage.SceneManager;
        if (scenes.Next is null)
        {
            return;
        }

        _logger.LogInformation("Unloading scene '{}'", scenes.Current.Name);
        stage.EventRegistry.Invoke<SceneUnloadEvent>(scenes.Current);

        scenes.Current = scenes.Next;
        scenes.Next = null;

        _logger.LogInformation("Loading scene '{}'", scenes.Current.Name);
        stage.EventRegistry.Invoke<SceneLoadEvent>(scenes.Current);

    }

    public void Update(GameTime gameTime)
    {
        var stage = _stages.CurrentStage;
        var scenes = stage.SceneManager;

        var profile = _systemProfiler.StartProfiling(nameof(Update));
        foreach (var (name, update) in stage.UpdateSystems)
        {
            update.Invoke(scenes.Current, gameTime);
            profile.Record(name);
        }
        profile.Stop();
    }

    public void Render(GameTime gameTime)
    {
        var stage = _stages.CurrentStage;
        var scenes = stage.SceneManager;

        var profile = _systemProfiler.StartProfiling(nameof(Render));
        foreach (var (name, renderer) in stage.RenderSystems)
        {
            renderer.Invoke(scenes.Current, gameTime);
            profile.Record(name);
        }
        profile.Stop();
    }

    public void ImGuiRender(GameTime gameTime)
    {
        var stage = _stages.CurrentStage;
        var scenes = stage.SceneManager;

        var profile = _systemProfiler.StartProfiling(nameof(ImGuiRender));
        foreach (var (name, gui) in stage.DebugUIs)
        {
            gui.Invoke(scenes.Current, gameTime);
            profile.Record(name);
        }
        profile.Stop();

        _systemProfiler.Render();
    }
}
