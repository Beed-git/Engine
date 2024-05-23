using Engine.Level;

namespace Engine.ECS;

internal class EventRunner<T>
    where T : struct
{
    private readonly List<Action<Scene, T>> _handlers;

    public EventRunner()
    {
        _handlers = [];
    }

    public void Register(Action<Scene, T> handler)
    {
        _handlers.Add(handler);
    }

    public void Invoke(Scene current, T @event)
    {
        foreach (var handler in _handlers)
        {
            handler.Invoke(current, @event);
        }
    }
}