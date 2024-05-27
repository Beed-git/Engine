using Engine.Files;
using Engine.Level;
using Microsoft.Extensions.Logging;
using System.Collections.Immutable;

namespace Engine.Core.Internal;

internal class Stage
{
    internal Stage(
        string name,
        SceneManager sceneManager,
        EventRegistry eventRegistry,
        IEnumerable<FrameUpdateSystem> updateSystems,
        IEnumerable<FrameUpdateSystem> renderSystems,
        IEnumerable<FrameUpdateSystem> debugUIs)
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
    internal IReadOnlyList<FrameUpdateSystem> UpdateSystems { get; private init; }
    internal IReadOnlyList<FrameUpdateSystem> RenderSystems { get; private init; }
    internal IReadOnlyList<FrameUpdateSystem> DebugUIs { get; private init; }
}
