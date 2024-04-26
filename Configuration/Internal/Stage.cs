using Microsoft.Xna.Framework;
using System.Collections.Immutable;

namespace Engine.Configuration.Internal;

internal class Stage
{
    internal Stage(
        string name,
        IEnumerable<Action> initSystems,
        IEnumerable<Action> destroySystems,
        IEnumerable<Action> onSceneLoadSystems,
        IEnumerable<Action> onSceneUnloadSystems,
        IEnumerable<Action<GameTime>> updateSystems,
        IEnumerable<Action<GameTime>> renderSystems,
        IEnumerable<Action<GameTime>> debugUIs)
    {
        Name = name;
        InitSystems = initSystems.ToImmutableList();
        DestroySystems = destroySystems.ToImmutableList();
        OnSceneLoadSystems = onSceneLoadSystems.ToImmutableList();
        OnSceneUnloadSystems = onSceneUnloadSystems.ToImmutableList();
        UpdateSystems = updateSystems.ToImmutableList();
        RenderSystems = renderSystems.ToImmutableList();
        DebugUIs = debugUIs.ToImmutableList();
    }

    public static Stage Empty => new("empty", [], [], [], [], [], [], []);

    public string Name { get; private init; }
    public IReadOnlyList<Action> InitSystems { get; private init; }
    public IReadOnlyList<Action> DestroySystems { get; private init; }
    public IReadOnlyList<Action> OnSceneLoadSystems { get; private init; }
    public IReadOnlyList<Action> OnSceneUnloadSystems { get; private init; }
    public IReadOnlyList<Action<GameTime>> UpdateSystems { get; private init; }
    public IReadOnlyList<Action<GameTime>> RenderSystems { get; private init; }
    public IReadOnlyList<Action<GameTime>> DebugUIs { get; private init; }
}
