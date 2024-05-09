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
        var profiler = _systemProfiler.StartProfiling($"{nameof(StageChange)}.Destroy");
        foreach (var (name, destroy) in stage.DestroySystems)
        {
            destroy.Invoke();
            profiler.Record(name);
        }
        profiler.Stop();

        stage = _repository.Create(_stages.Next);
        _stages.CurrentStage = stage;
        _stages.Next = null;

        _logger.LogInformation("Initializing stage '{}'", stage.Name);
        profiler = _systemProfiler.StartProfiling($"{nameof(StageChange)}.Initialize");
        foreach (var (name, init) in stage.InitSystems)
        {
            init.Invoke();
            profiler.Record(name);
        }
        profiler.Stop();
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
        var profiler = _systemProfiler.StartProfiling($"{nameof(SceneChange)}.Unload");
        foreach (var (name, unload) in stage.OnSceneUnloadSystems)
        {
            unload.Invoke(scenes.Current);
            profiler.Record(name);
        }
        profiler.Stop();

        scenes.Current = scenes.Next;
        scenes.Next = null;

        _logger.LogInformation("Loading scene '{}'", scenes.Current.Name);
        profiler = _systemProfiler.StartProfiling($"{nameof(SceneChange)}.Load");
        foreach (var (name, load) in stage.OnSceneLoadSystems)
        {
            load.Invoke(scenes.Current);
            profiler.Record(name);
        }
        profiler.Stop();
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
