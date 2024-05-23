using Engine.Level;

namespace Engine.ECS;

public class EventSystem
{
    private readonly EventRegistry _events;
    private readonly List<Action<Scene>> _queue;

    internal EventSystem(EventRegistry events)
    {
        _events = events;
        _queue = [];
    }

    public void Queue<T>(T payload)
        where T : struct
    {
        var @event = (Scene scene) => _events.Invoke(scene, payload);
        _queue.Add(@event);
    }

    internal void RunEvents(Scene current)
    {
        foreach (var action in _queue)
        {
            action.Invoke(current);
        }

        _queue.Clear();
    }
}
