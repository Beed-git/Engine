using Engine.Level;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace Engine.Configuration;

public class StageConfig
{
    private readonly List<KeyValuePair<string, Action>> _initSystems;
    private readonly List<KeyValuePair<string, Action>> _destroySystems;
    private readonly List<KeyValuePair<string, Action<Scene>>> _onSceneLoadSystems;
    private readonly List<KeyValuePair<string, Action<Scene>>> _onSceneUnloadSystems;
    private readonly List<KeyValuePair<string, Action<Scene, GameTime>>> _updateSystems;
    private readonly List<KeyValuePair<string, Action<Scene, GameTime>>> _renderSystems;
    private readonly List<KeyValuePair<string, Action<Scene, GameTime>>> _debugUIs;

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
    internal IEnumerable<KeyValuePair<string, Action>> InitSystems => _initSystems;
    internal IEnumerable<KeyValuePair<string, Action>> DestroySystems => _destroySystems;
    internal IEnumerable<KeyValuePair<string, Action<Scene>>> OnSceneLoadSystems => _onSceneLoadSystems;
    internal IEnumerable<KeyValuePair<string, Action<Scene>>> OnSceneUnloadSystems => _onSceneUnloadSystems;
    internal IEnumerable<KeyValuePair<string, Action<Scene, GameTime>>> DebugUIs => _debugUIs;
    internal IEnumerable<KeyValuePair<string, Action<Scene, GameTime>>> UpdateSystems => _updateSystems;
    internal IEnumerable<KeyValuePair<string, Action<Scene, GameTime>>> RenderSystems => _renderSystems;

    // Initialization/Disposal

    public void RegisterInitializationSystem(Action system)
        => RegisterInitializationSystem(GetName(system.Method), system);

    private void RegisterInitializationSystem(string name, Action system)
    {
        _initSystems.Add(new (name, system));
    }

    public void RegisterDestroySystem(Action system)
        => RegisterDestroySystem(GetName(system.Method), system);

    private void RegisterDestroySystem(string name, Action system)
    {
        _destroySystems.Add(new (name, system));
    }

    // Scene load/unload

    public void RegisterOnSceneLoadSystem(Action system) 
        => RegisterOnSceneLoadSystem(GetName(system.Method), (Scene _) => system.Invoke());
    public void RegisterOnSceneLoadSystem(Action<Scene> system)
        => RegisterOnSceneLoadSystem(GetName(system.Method), system);

    private void RegisterOnSceneLoadSystem(string name, Action<Scene> action)
    {
        _onSceneLoadSystems.Add(new (name, action));
    }

    public void RegisterOnSceneUnloadSystem(Action system) 
        => RegisterOnSceneUnloadSystem(GetName(system.Method), (Scene _) => system.Invoke());
    public void RegisterOnSceneUnloadSystem(Action<Scene> system)
        => RegisterOnSceneUnloadSystem(GetName(system.Method), system);

    private void RegisterOnSceneUnloadSystem(string name, Action<Scene> system)
    {
        _onSceneUnloadSystems.Add(new (name, system));
    }

    // Update

    public void RegisterUpdateSystem(Action system) 
        => RegisterUpdateSystem(GetName(system.Method), (Scene _, GameTime _) => system.Invoke());
    public void RegisterUpdateSystem(Action<Scene> system) 
        => RegisterUpdateSystem(GetName(system.Method), (Scene scene, GameTime _) => system.Invoke(scene));
    public void RegisterUpdateSystem(Action<GameTime> system) 
        => RegisterUpdateSystem(GetName(system.Method), (Scene _, GameTime gameTime) => system.Invoke(gameTime));
    public void RegisterUpdateSystem(Action<Scene, GameTime> system) 
        => RegisterUpdateSystem(GetName(system.Method), system);

    private void RegisterUpdateSystem(string name, Action<Scene, GameTime> system)
    {
        _updateSystems.Add(new (name, system));
    }

    // Render

    public void RegisterRenderSystem(Action system) 
        => RegisterRenderSystem(GetName(system.Method), (Scene _, GameTime _) => system.Invoke());
    public void RegisterRenderSystem(Action<Scene> system) 
        => RegisterRenderSystem(GetName(system.Method), (Scene scene, GameTime _) => system.Invoke(scene));
    public void RegisterRenderSystem(Action<GameTime> system) 
        => RegisterRenderSystem(GetName(system.Method), (Scene _, GameTime gameTime) => system.Invoke(gameTime));
    public void RegisterRenderSystem(Action<Scene, GameTime> system) 
        => RegisterRenderSystem(GetName(system.Method), system);

    private void RegisterRenderSystem(string name, Action<Scene, GameTime> system)
    {
        _renderSystems.Add(new (name, system));
    }

    // Debug UI

    public void RegisterDebugUI(Action system) 
        => RegisterDebugUI(GetName(system.Method), (Scene _, GameTime _) => system.Invoke());
    public void RegisterDebugUI(Action<Scene> system) 
        => RegisterDebugUI(GetName(system.Method), (Scene scene, GameTime _) => system.Invoke(scene));
    public void RegisterDebugUI(Action<GameTime> system) 
        => RegisterDebugUI(GetName(system.Method), (Scene _, GameTime gameTime) => system.Invoke(gameTime));
    public void RegisterDebugUI(Action<Scene, GameTime> system)
        => RegisterDebugUI(GetName(system.Method), system);
    
    private void RegisterDebugUI(string name, Action<Scene, GameTime> system)
    {
        _debugUIs.Add(new (name, system));
    }

    // Helpers
    private static string GetName(MethodInfo method)
    {
        if (method.DeclaringType is not null)
        {
            // Check if not an anonymous type.
            if (method.DeclaringType.Name.Contains('<'))
            {
                return $"(anonymous) {method.Name}";
            }
            else
            {
                return $"{method.DeclaringType.Name}::{method.Name}";
            }
        }
        else
        {
            return $"{method.Name}";
        }
    }
}