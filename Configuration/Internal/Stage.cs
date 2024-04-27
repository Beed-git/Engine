using Engine.Files;
using Engine.Level;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System.Collections.Immutable;

namespace Engine.Configuration.Internal;

internal class Stage
{
    internal Stage(
        string name,
        SceneManager sceneManager,
        IEnumerable<Action> initSystems,
        IEnumerable<Action> destroySystems,
        IEnumerable<Action> onSceneLoadSystems,
        IEnumerable<Action> onSceneUnloadSystems,
        IEnumerable<Action<GameTime>> updateSystems,
        IEnumerable<Action<GameTime>> renderSystems,
        IEnumerable<Action<GameTime>> debugUIs)
    {
        Name = name;
        SceneManager = sceneManager;
        InitSystems = initSystems.ToImmutableList();
        DestroySystems = destroySystems.ToImmutableList();
        OnSceneLoadSystems = onSceneLoadSystems.ToImmutableList();
        OnSceneUnloadSystems = onSceneUnloadSystems.ToImmutableList();
        UpdateSystems = updateSystems.ToImmutableList();
        RenderSystems = renderSystems.ToImmutableList();
        DebugUIs = debugUIs.ToImmutableList();
    }

    public static Stage CreateEmpty(ILoggerFactory loggerFactory, FileSystem files)
    {
        var scenes = new SceneManager(loggerFactory, files);
        var stage = new Stage("empty", scenes, [], [], [], [], [], [], []);
        return stage;
    } 

    public string Name { get; private init; }
    public SceneManager SceneManager { get; private init; }
    public IReadOnlyList<Action> InitSystems { get; private init; }
    public IReadOnlyList<Action> DestroySystems { get; private init; }
    public IReadOnlyList<Action> OnSceneLoadSystems { get; private init; }
    public IReadOnlyList<Action> OnSceneUnloadSystems { get; private init; }
    public IReadOnlyList<Action<GameTime>> UpdateSystems { get; private init; }
    public IReadOnlyList<Action<GameTime>> RenderSystems { get; private init; }
    public IReadOnlyList<Action<GameTime>> DebugUIs { get; private init; }
}
