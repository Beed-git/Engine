using Engine.Core.Utility;
using Engine.Level;
using System.Diagnostics;

namespace Engine.Core.Internal;

internal interface IEventRunner
{
    public IEnumerable<KeyValuePair<string, Profiler>> HandlerProfilers { get; }
}

internal class EventRunner<T>
    : IEventRunner
    where T : struct
{
    private readonly Stopwatch _timer;
    private readonly List<EventHandler<T>> _handlers;

    public EventRunner()
    {
        _handlers = [];
        _timer = new Stopwatch();
    }

    public IEnumerable<KeyValuePair<string, Profiler>> HandlerProfilers 
        => _handlers.Select(h => new KeyValuePair<string, Profiler>(h.Name, h.Profiler));

    public void Register(EventHandler<T> handler)
    {
        _handlers.Add(handler);
    }

    public void Invoke(Scene current, T @event)
    {
        foreach (var handler in _handlers)
        {
            _timer.Start();
            handler.Action.Invoke(current, @event);

            _timer.Stop();
            handler.Profiler.Record(_timer.Elapsed);
            _timer.Reset();
        }
    }
}