using Engine.ECS.Events;
using Engine.Level;
using ImGuiNET;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Engine.Core.Internal;

internal class SystemManager
{
    private readonly ILogger _logger;
    private readonly StageRepository _repository;
    private readonly StageManager _stages;
    private readonly Stopwatch _profiler;

    internal SystemManager(ILoggerFactory loggerFactory, StageRepository repository, StageManager stages)
    {
        _logger = loggerFactory.CreateLogger<SystemManager>();
        _repository = repository;
        _stages = stages;
        _profiler = new Stopwatch();
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
        FrameUpdate(stage.UpdateSystems, gameTime);
    }

    public void Render(GameTime gameTime)
    {
        var stage = _stages.CurrentStage;
        FrameUpdate(stage.RenderSystems, gameTime);
    }

    public void ImGuiRender(GameTime gameTime)
    {
        var stage = _stages.CurrentStage;
        FrameUpdate(stage.DebugUIs, gameTime);
    }

    private void FrameUpdate(IEnumerable<FrameUpdateSystem> systems, GameTime gameTime)
    {
        var current = _stages.CurrentStage.SceneManager.Current;
        foreach (var (name, system, stats) in systems)
        {
            _profiler.Start();

            system.Invoke(current, gameTime);

            _profiler.Stop();
            stats.Record(_profiler.Elapsed);
            _profiler.Reset();
        }
    }
}