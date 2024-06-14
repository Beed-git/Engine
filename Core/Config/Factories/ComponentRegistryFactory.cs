using Engine.ECS;
using Engine.Serialization;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Engine.Core.Config.Factories;

internal static class ComponentRegistryFactory
{
    internal static ComponentRegistry Create(ComponentConfig config, ILoggerFactory loggerFactory)
    {
        var registry = new ComponentRegistry(loggerFactory);
        if (config.IsAutoRegistered)
        {
            AutoRegisterComponents(registry);
        }
        return registry;
    }

    private static void AutoRegisterComponents(ComponentRegistry registry)
    {
        const string ComponentEnding = "Component";

        var types =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.IsDefined(typeof(ComponentAttribute)))
            .Concat(
                Assembly.GetEntryAssembly()?
                    .GetTypes()
                    .Where(t => t.IsDefined(typeof(ComponentAttribute))) ?? []);

        foreach (var type in types)
        {
            var name = type.Name;
            if (name.EndsWith(ComponentEnding, StringComparison.OrdinalIgnoreCase))
            {
                name = name[..^ComponentEnding.Length];
            }

            if (name.Length == 0)
            {
                throw new Exception($"Generated name is invalid for struct '{type}' (name is '{type}')");
            }

            name = char.ToLower(name[0]) + name[1..];
            registry.Register(type, name);
        }
    }
}
