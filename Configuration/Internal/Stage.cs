using Engine.Level;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using System.Collections.Immutable;

namespace Engine.Configuration.Internal;

internal class Stage
{
    internal Stage(
        string name,
        ILoggerFactory loggerFactory,
        IEnumerable<Action<GameTime>> updateSystems,
        IEnumerable<Action<GameTime>> renderSystems,
        IEnumerable<Action<GameTime>> debugUIs)
    {
        Name = name;
        SceneManager = new SceneManager(loggerFactory);
        UpdateSystems = updateSystems.ToImmutableList();
        RenderSystems = renderSystems.ToImmutableList();
        DebugUIs = debugUIs.ToImmutableList();
    }

    public string Name { get; private init; }
    public IReadOnlyList<Action<GameTime>> UpdateSystems { get; private init; }
    public IReadOnlyList<Action<GameTime>> RenderSystems { get; private init; }
    public IReadOnlyList<Action<GameTime>> DebugUIs { get; private init; }
    public SceneManager SceneManager { get; private init; }
}
