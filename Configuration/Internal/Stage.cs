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
        IEnumerable<KeyValuePair<string, Action>> initSystems,
        IEnumerable<KeyValuePair<string, Action>> destroySystems,
        IEnumerable<KeyValuePair<string, Action<Scene>>> onSceneLoadSystems,
        IEnumerable<KeyValuePair<string, Action<Scene>>> onSceneUnloadSystems,
        IEnumerable<KeyValuePair<string, Action<Scene, GameTime>>> updateSystems,
        IEnumerable<KeyValuePair<string, Action<Scene, GameTime>>> renderSystems,
        IEnumerable<KeyValuePair<string, Action<Scene, GameTime>>> debugUIs)
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
    public IReadOnlyList<KeyValuePair<string, Action>> InitSystems { get; private init; }
    public IReadOnlyList<KeyValuePair<string, Action>> DestroySystems { get; private init; }
    public IReadOnlyList<KeyValuePair<string, Action<Scene>>> OnSceneLoadSystems { get; private init; }
    public IReadOnlyList<KeyValuePair<string, Action<Scene>>> OnSceneUnloadSystems { get; private init; }
    public IReadOnlyList<KeyValuePair<string, Action<Scene, GameTime>>> UpdateSystems { get; private init; }
    public IReadOnlyList<KeyValuePair<string, Action<Scene, GameTime>>> RenderSystems { get; private init; }
    public IReadOnlyList<KeyValuePair<string, Action<Scene, GameTime>>> DebugUIs { get; private init; }
}
