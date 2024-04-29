using Engine.Level;
using Microsoft.Xna.Framework;

namespace Engine.Configuration;

public class StageConfig
{
    private readonly List<Action> _initSystems;
    private readonly List<Action> _destroySystems;
    private readonly List<Action<Scene>> _onSceneLoadSystems;
    private readonly List<Action<Scene>> _onSceneUnloadSystems;
    private readonly List<Action<Scene, GameTime>> _updateSystems;
    private readonly List<Action<Scene, GameTime>> _renderSystems;
    private readonly List<Action<Scene, GameTime>> _debugUIs;

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
    internal IEnumerable<Action<Scene>> OnSceneLoadSystems => _onSceneLoadSystems;
    internal IEnumerable<Action<Scene>> OnSceneUnloadSystems => _onSceneUnloadSystems;
    internal IEnumerable<Action<Scene, GameTime>> DebugUIs => _debugUIs;
    internal IEnumerable<Action<Scene, GameTime>> UpdateSystems => _updateSystems;
    internal IEnumerable<Action<Scene, GameTime>> RenderSystems => _renderSystems;

    // Initialization/Disposal

    public void RegisterInitializationSystem(Action system)
    {
        _initSystems.Add(system);
    }

    public void RegisterDestroySystem(Action system)
    {
        _destroySystems.Add(system);
    }

    // Scene load/unload

    public void RegisterOnSceneLoadSystem(Action action) => RegisterOnSceneLoadSystem((Scene _) => action.Invoke());
    public void RegisterOnSceneLoadSystem(Action<Scene> action)
    {
        _onSceneLoadSystems.Add(action);
    }

    public void RegisterOnSceneUnloadSystem(Action action) => RegisterOnSceneUnloadSystem((Scene _) => action.Invoke());
    public void RegisterOnSceneUnloadSystem(Action<Scene> action)
    {
        _onSceneUnloadSystems.Add(action);
    }

    // Update

    public void RegisterUpdateSystem(Action system) => RegisterUpdateSystem((Scene _, GameTime _) => system.Invoke());
    public void RegisterUpdateSystem(Action<Scene> system) => RegisterUpdateSystem((Scene scene, GameTime _) => system.Invoke(scene));
    public void RegisterUpdateSystem(Action<GameTime> system) => RegisterUpdateSystem((Scene _, GameTime gameTime) => system.Invoke(gameTime));

    public void RegisterUpdateSystem(Action<Scene, GameTime> system)
    {
        _updateSystems.Add(system);
    }

    // Render

    public void RegisterRenderSystem(Action system) => RegisterRenderSystem((Scene _, GameTime _) => system.Invoke());
    public void RegisterRenderSystem(Action<Scene> system) => RegisterRenderSystem((Scene scene, GameTime _) => system.Invoke(scene));
    public void RegisterRenderSystem(Action<GameTime> system) => RegisterRenderSystem((Scene _, GameTime gameTime) => system.Invoke(gameTime));

    public void RegisterRenderSystem(Action<Scene, GameTime> system)
    {
        _renderSystems.Add(system);
    }

    // Debug UI

    public void RegisterDebugUI(Action system) => RegisterDebugUI((Scene _, GameTime _) => system.Invoke());
    public void RegisterDebugUI(Action<Scene> system) => RegisterDebugUI((Scene scene, GameTime _) => system.Invoke(scene));
    public void RegisterDebugUI(Action<GameTime> system) => RegisterDebugUI((Scene _, GameTime gameTime) => system.Invoke(gameTime));

    public void RegisterDebugUI(Action<Scene, GameTime> system)
    {
        _debugUIs.Add(system);
    }

}