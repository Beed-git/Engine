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
        IEnumerable<Action<Scene>> onSceneLoadSystems,
        IEnumerable<Action<Scene>> onSceneUnloadSystems,
        IEnumerable<Action<Scene, GameTime>> updateSystems,
        IEnumerable<Action<Scene, GameTime>> renderSystems,
        IEnumerable<Action<Scene, GameTime>> debugUIs)
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
    public IReadOnlyList<Action<Scene>> OnSceneLoadSystems { get; private init; }
    public IReadOnlyList<Action<Scene>> OnSceneUnloadSystems { get; private init; }
    public IReadOnlyList<Action<Scene, GameTime>> UpdateSystems { get; private init; }
    public IReadOnlyList<Action<Scene, GameTime>> RenderSystems { get; private init; }
    public IReadOnlyList<Action<Scene, GameTime>> DebugUIs { get; private init; }
}
