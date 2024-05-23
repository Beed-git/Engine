using Engine.Level;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Engine.ECS;

internal class EventRegistry
{
    private readonly ILogger<EventRegistry> _logger;

    private readonly Dictionary<Type, object> _runners;

    public EventRegistry(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<EventRegistry>();

        _runners = [];
    }

    internal void RegisterEventHandler<T>(string name, Action<Scene, T> handler)
        where T : struct
    {
        var type = typeof(T);
        if (_runners.TryGetValue(type, out var obj))
        {
            if (obj is not EventRunner<T> runner)
            {
                _logger.LogError("Failed to register event handler. Expected handler to be a EventHandler<{}> but was actually {}", type, obj.GetType());
                return;
            }

            runner.Register(handler);
        }
        else
        {
            _logger.LogInformation("Event handler of type {} does not exist. Creating event handler.", type);

            var runner = new EventRunner<T>();
            runner.Register(handler);

            _runners.Add(type, runner);
        }

    }

    internal void Invoke<T>(Scene current)
        where T : struct
    {
        Invoke<T>(current, default);
    }

    internal void Invoke<T>(Scene current, T payload)
        where T : struct
    {
        if (!TryGetRunner<T>(out var runner))
        {
            return;
        }

        runner.Invoke(current, payload);
    }

    internal bool TryGetRunner<T>([MaybeNullWhen(false)] out EventRunner<T> runner)
        where T : struct
    {
        var type = typeof(T);
        if (!_runners.TryGetValue(typeof(T), out var obj))
        {
            _logger.LogInformation("Event runner of type {} does not exist. Creating event runner.", typeof(T));
            
            runner = new EventRunner<T>();
            _runners.Add(typeof(T), runner);

            return true;
        }
        
        // This path theoretically should never happen.
        if (obj is not EventRunner<T> r)
        {
            var exception = new InvalidOperationException($"{nameof(obj)} is invalid type.");
            _logger.LogError(exception, string.Empty, $"Expected handler to be a EventHandler<{type}> but was actually {obj.GetType()}");
            throw exception;
        }

        runner = r;
        return true;
    }
}