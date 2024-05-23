using Engine.ECS;
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
        EventRegistry eventRegistry,
        IEnumerable<KeyValuePair<string, Action<Scene, GameTime>>> updateSystems,
        IEnumerable<KeyValuePair<string, Action<Scene, GameTime>>> renderSystems,
        IEnumerable<KeyValuePair<string, Action<Scene, GameTime>>> debugUIs)
    {
        Name = name;
        SceneManager = sceneManager;
        EventRegistry = eventRegistry;
        UpdateSystems = updateSystems.ToImmutableList();
        RenderSystems = renderSystems.ToImmutableList();
        DebugUIs = debugUIs.ToImmutableList();
    }

    public static Stage CreateEmpty(ILoggerFactory loggerFactory, FileSystem files)
    {
        var events = new EventRegistry(loggerFactory);
        var scenes = new SceneManager(loggerFactory, files);
        var stage = new Stage("empty", scenes, events, [], [], []);
        return stage;
    } 

    public string Name { get; private init; }
    internal SceneManager SceneManager { get; private init; }
    internal EventRegistry EventRegistry { get; private init; }
    internal IReadOnlyList<KeyValuePair<string, Action<Scene, GameTime>>> UpdateSystems { get; private init; }
    internal IReadOnlyList<KeyValuePair<string, Action<Scene, GameTime>>> RenderSystems { get; private init; }
    internal IReadOnlyList<KeyValuePair<string, Action<Scene, GameTime>>> DebugUIs { get; private init; }
}
