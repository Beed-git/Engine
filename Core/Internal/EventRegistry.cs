using Engine.Level;
using Microsoft.Extensions.Logging;

namespace Engine.Core.Internal;

internal class EventRegistry
{
    private readonly ILogger<EventRegistry> _logger;
    private readonly Dictionary<Type, IEventRunner> _runners;

    public EventRegistry(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<EventRegistry>();
        _runners = [];
    }

    internal IEnumerable<KeyValuePair<Type, IEventRunner>> Runners => _runners;

    internal void RegisterEventHandler<T>(string name, Action<Scene, T> action)
        where T : struct
    {
        var runner = GetRunner<T>();
        var handler = new EventHandler<T>(name, action);
        runner.Register(handler);
    }

    internal void Invoke<T>(Scene current)
        where T : struct
    {
        Invoke<T>(current, default);
    }

    internal void Invoke<T>(Scene current, T payload)
        where T : struct
    {
        var runner = GetRunner<T>();
        runner.Invoke(current, payload);
    }

    private EventRunner<T> GetRunner<T>()
        where T : struct
    {
        var type = typeof(T);
        if (_runners.TryGetValue(type, out var obj))
        {
            if (obj is not EventRunner<T> runner)
            {
                throw new InvalidOperationException($"Failed to register event handler. Expected handler to be a EventHandler<{type}> but was actually {obj.GetType()}");
            }

            return runner;
        }
        else
        {
            _logger.LogInformation("Event handler of type {} does not exist. Creating event handler.", type);
            var runner = CreateRunner<T>();
            return runner;
        }
    }

    private EventRunner<T> CreateRunner<T>()
        where T : struct
    {
        var type = typeof(T);
        var runner = new EventRunner<T>();
        _runners.Add(type, runner);
        return runner;
    }
}