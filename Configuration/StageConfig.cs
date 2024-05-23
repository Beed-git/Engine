﻿using Engine.ECS;
using Engine.Level;
using Microsoft.Xna.Framework;
using System.Reflection;

namespace Engine.Configuration;

public class StageConfig
{
    private readonly List<KeyValuePair<string, Action<EventRegistry>>> _eventRegisterSystems;
    private readonly List<KeyValuePair<string, Action<Scene, GameTime>>> _updateSystems;
    private readonly List<KeyValuePair<string, Action<Scene, GameTime>>> _renderSystems;
    private readonly List<KeyValuePair<string, Action<Scene, GameTime>>> _debugUIs;

    public StageConfig(string name)
    {
        Name = name;
        _debugUIs = [];
        _eventRegisterSystems = [];
        _updateSystems = [];
        _renderSystems = [];
    }

    public string Name { get; private init; }
    internal IEnumerable<KeyValuePair<string, Action<Scene, GameTime>>> DebugUIs => _debugUIs;
    internal IEnumerable<KeyValuePair<string, Action<Scene, GameTime>>> UpdateSystems => _updateSystems;
    internal IEnumerable<KeyValuePair<string, Action<EventRegistry>>> EventRegisterSystems => _eventRegisterSystems;
    internal IEnumerable<KeyValuePair<string, Action<Scene, GameTime>>> RenderSystems => _renderSystems;

    // Update

    public void RegisterUpdateSystem(Action system) 
        => RegisterUpdateSystemInternal(GetName(system.Method), (Scene _, GameTime _) => system.Invoke());
    public void RegisterUpdateSystem(Action<Scene> system) 
        => RegisterUpdateSystemInternal(GetName(system.Method), (Scene scene, GameTime _) => system.Invoke(scene));
    public void RegisterUpdateSystem(Action<GameTime> system) 
        => RegisterUpdateSystemInternal(GetName(system.Method), (Scene _, GameTime gameTime) => system.Invoke(gameTime));
    public void RegisterUpdateSystem(Action<Scene, GameTime> system) 
        => RegisterUpdateSystemInternal(GetName(system.Method), system);

    private void RegisterUpdateSystemInternal(string name, Action<Scene, GameTime> system)
    {
        _updateSystems.Add(new (name, system));
    }

    // Events 
    public void RegisterEventHandler<T>(Action handler)
        where T : struct
        => RegisterEventHandlerInternal(GetName(handler.Method), (Scene _, T _) => handler.Invoke());
    public void RegisterEventHandler<T>(Action<T> handler)
        where T : struct
        => RegisterEventHandlerInternal(GetName(handler.Method), (Scene _, T payload) => handler.Invoke(payload));
    public void RegisterEventHandler<T>(Action<Scene> handler)
        where T : struct
        => RegisterEventHandlerInternal(GetName(handler.Method), (Scene scene, T _) => handler.Invoke(scene));
    public void RegisterEventHandler<T>(Action<Scene, T> handler)
        where T : struct
        => RegisterEventHandlerInternal(GetName(handler.Method), handler);

    private void RegisterEventHandlerInternal<T>(string name, Action<Scene, T> handler)
        where T : struct
    {
        var initializer = (EventRegistry events) => events.RegisterEventHandler(name, handler);
        _eventRegisterSystems.Add(new(name, initializer));
    }

    // Render

    public void RegisterRenderSystem(Action system) 
        => RegisterRenderSystemInternal(GetName(system.Method), (Scene _, GameTime _) => system.Invoke());
    public void RegisterRenderSystem(Action<Scene> system) 
        => RegisterRenderSystemInternal(GetName(system.Method), (Scene scene, GameTime _) => system.Invoke(scene));
    public void RegisterRenderSystem(Action<GameTime> system) 
        => RegisterRenderSystemInternal(GetName(system.Method), (Scene _, GameTime gameTime) => system.Invoke(gameTime));
    public void RegisterRenderSystem(Action<Scene, GameTime> system) 
        => RegisterRenderSystemInternal(GetName(system.Method), system);

    private void RegisterRenderSystemInternal(string name, Action<Scene, GameTime> system)
    {
        _renderSystems.Add(new (name, system));
    }

    // Debug UI

    public void RegisterDebugUI(Action system) 
        => RegisterDebugUIInternal(GetName(system.Method), (Scene _, GameTime _) => system.Invoke());
    public void RegisterDebugUI(Action<Scene> system) 
        => RegisterDebugUIInternal(GetName(system.Method), (Scene scene, GameTime _) => system.Invoke(scene));
    public void RegisterDebugUI(Action<GameTime> system) 
        => RegisterDebugUIInternal(GetName(system.Method), (Scene _, GameTime gameTime) => system.Invoke(gameTime));
    public void RegisterDebugUI(Action<Scene, GameTime> system)
        => RegisterDebugUIInternal(GetName(system.Method), system);
    
    private void RegisterDebugUIInternal(string name, Action<Scene, GameTime> system)
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