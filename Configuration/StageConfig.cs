using Microsoft.Xna.Framework;

namespace Engine.Configuration;

public class StageConfig
{
    private readonly List<Action> _initSystems;
    private readonly List<Action> _destroySystems;
    private readonly List<Action> _onSceneLoadSystems;
    private readonly List<Action> _onSceneUnloadSystems;
    private readonly List<Action<GameTime>> _updateSystems;
    private readonly List<Action<GameTime>> _renderSystems;
    private readonly List<Action<GameTime>> _debugUIs;

    public StageConfig(string name)
    {
        Name = name;
        _initSystems = [];
        _destroySystems = [];
        _onSceneLoadSystems = [];
        _onSceneUnloadSystems = [];
        _debugUIs = [];
        _updateSystems = [];
        _renderSystems = [];
    }

    public string Name { get; private init; }
    internal IEnumerable<Action> InitSystems => _initSystems;
    internal IEnumerable<Action> DestroySystems => _destroySystems;
    internal IEnumerable<Action> OnSceneLoadSystems => _onSceneLoadSystems;
    internal IEnumerable<Action> OnSceneUnloadSystems => _onSceneUnloadSystems;
    internal IEnumerable<Action<GameTime>> DebugUIs => _debugUIs;
    internal IEnumerable<Action<GameTime>> UpdateSystems => _updateSystems;
    internal IEnumerable<Action<GameTime>> RenderSystems => _renderSystems;

    public void RegisterInitializationSystem(Action system)
    {
        _initSystems.Add(system);
    }

    public void RegisterDestroySystem(Action system)
    {
        _destroySystems.Add(system);
    }

    public void RegisterOnSceneLoadSystem(Action action)
    {
        _onSceneLoadSystems.Add(action);
    }

    public void RegisterOnSceneUnloadSystem(Action action)
    {
        _onSceneUnloadSystems.Add(action);
    }

    public void RegisterUpdateSystem(Action system)
    {
        RegisterDebugUI((GameTime _) => system.Invoke());
    }

    public void RegisterUpdateSystem(Action<GameTime> system)
    {
        _updateSystems.Add(system);
    }

    public void RegisterRenderSystem(Action system)
    {
        RegisterRenderSystem((GameTime _) => system.Invoke());
    }

    public void RegisterRenderSystem(Action<GameTime> system)
    {
        _renderSystems.Add(system);
    }

    public void RegisterDebugUI(Action system)
    {
        RegisterDebugUI((GameTime _) => system.Invoke());
    }

    public void RegisterDebugUI(Action<GameTime> system)
    {
        _debugUIs.Add(system);
    }

}