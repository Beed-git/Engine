using Engine.Core.Internal;
using Engine.Level;
using Engine.Maths.Hexagons;

namespace Engine.ECS;

public class EventSystem
{
    private readonly EventRegistry _events;
    private readonly Queue<Action<Scene>> _queue;

    internal EventSystem(EventRegistry events)
    {
        _events = events;
        _queue = [];
    }

    public void Queue<T>()
        where T : struct
    {
        Queue<T>(default);
    }

    public void Queue<T>(T payload)
        where T : struct
    {
        var @event = (Scene scene) => _events.Invoke(scene, payload);
        _queue.Enqueue(@event);
    }

    internal void RunEvents(Scene current)
    {
        while (_queue.TryDequeue(out var action))
        {
            action.Invoke(current);
        }
    }
}
