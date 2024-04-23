using Microsoft.Extensions.Logging;

namespace Engine.Serialization;

public class ComponentRegistry
{
    private readonly ILogger<ComponentRegistry> _logger;

    private readonly Dictionary<Type, string> _typesToNames;
    private readonly Dictionary<string, Type> _namesToTypes;

    public ComponentRegistry(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ComponentRegistry>();

        _namesToTypes = [];
        _typesToNames = [];
    }

    public void Register<T>(string name)
        where T : struct
    {
        var type = typeof(T);
        _namesToTypes.Add(name, type);
        _typesToNames.Add(type, name);
    }

    public void Register(Type type, string name)
    {
        if (!type.IsValueType)
        {
            _logger.LogError("Type '{}' with name '{}' is not a struct.", type, name);
            return;
        }

        _namesToTypes.Add(name, type);
        _typesToNames.Add(type, name);
    }

    public bool TryGetName(Type type, out string name)
    {
        if (_typesToNames.TryGetValue(type, out name!))
        {
            return true;
        }

        _logger.LogWarning("Failed to get name of component, type '{}' is not registered as a component.", type.Name);
        name = string.Empty;
        return false;
    }

    public bool TryGetType(string name, out Type type)
    {
        if (_namesToTypes.TryGetValue(name, out type!))
        {
            return true;
        }

        _logger.LogWarning("Failed to find any component with the name '{}'", name);
        return false;
    }
}
