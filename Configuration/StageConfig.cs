using Microsoft.Xna.Framework;

namespace Engine.Configuration;

public class StageConfig
{
    private readonly List<Action<GameTime>> _debugUIs;
    private readonly List<Action<GameTime>> _updateSystems;
    private readonly List<Action<GameTime>> _renderSystems;

    private readonly HashSet<Type> _registeredSystems;

    public StageConfig(string name)
    {
        Name = name;
        _debugUIs = [];
        _updateSystems = [];
        _renderSystems = [];

        _registeredSystems = [];
    }

    public string Name { get; private init; }
    internal IEnumerable<Action<GameTime>> DebugUIs => _debugUIs;
    internal IEnumerable<Action<GameTime>> UpdateSystems => _updateSystems;
    internal IEnumerable<Action<GameTime>> RenderSystems => _renderSystems;

    public void RegisterDebugUI(Action system)
    {
        RegisterDebugUI((GameTime _) => system.Invoke());
    }
    
    public void RegisterDebugUI(Action<GameTime> system)
    {
        Validate(system);
        _debugUIs.Add(system);
    }

    public void RegisterUpdateSystem(Action system)
    {
        RegisterDebugUI((GameTime _) => system.Invoke());
    }

    public void RegisterUpdateSystem(Action<GameTime> system)
    {
        Validate(system);
        _updateSystems.Add(system);
    }

    public void RegisterRenderSystem(Action system)
    {
        RegisterRenderSystem((GameTime _) => system.Invoke());
    }

    public void RegisterRenderSystem(Action<GameTime> system)
    {
        Validate(system);
        _renderSystems.Add(system);
    }

    private void Validate(Action<GameTime> system)
    {
        //var type = system.GetType();
        //if (_registeredSystems.Contains(type))
        //{
        //    throw new Exception($"System with type '{type.Name}' is already registered.");
        //}
        //_registeredSystems.Add(type);
    }
}