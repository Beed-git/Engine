using Engine.Serialization.Converters;
using Engine.Serialization;
using System.Text.Json;

namespace Engine.Core.Config.Factories;

internal static class SerializerFactory
{
    internal static Serializer Create(SerializationConfig config, ComponentRegistry registry)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        foreach (var converter in config.Converters)
        {
            jsonSerializerOptions.Converters.Add(converter);
        }

        if (config.UseDefaultConverters)
        {
            jsonSerializerOptions.Converters.Add(new TemplateConverter(registry));
        }

        var serializer = new Serializer(jsonSerializerOptions);
        return serializer;
    }
}
