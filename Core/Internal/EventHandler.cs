using Engine.Core.Utility;
using Engine.Level;

namespace Engine.Core.Internal;

internal class EventHandler<T>
    : IEventHandler
{
    public EventHandler(string name, Action<Scene, T> action)
    {
        Name = name;
        Action = action;
        Profiler = new Profiler();
    }

    public string Name { get; private init; }
    public Profiler Profiler { get; private init; }
    public Action<Scene, T> Action { get; private init; }
}
