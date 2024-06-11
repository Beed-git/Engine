using Engine.Serialization.Converters;
using Engine.Serialization;
using System.Text.Json;

namespace Engine.Core.Config.Factories;

internal static class SerializerFactory
{
    internal static Serializer Create(SerializationConfig config, ComponentRegistry registry)
    {
        var allowTrailingCommas = true;
        var commentHandling = JsonCommentHandling.Skip;


        var jsonSerializerOptions = new JsonSerializerOptions
        {
            AllowTrailingCommas = allowTrailingCommas,
            ReadCommentHandling = commentHandling,
            WriteIndented = true,
            IncludeFields = true,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var jsonReaderOptions = new JsonReaderOptions
        {
            AllowTrailingCommas = allowTrailingCommas,
            CommentHandling = commentHandling,
        };

        foreach (var converter in config.Converters)
        {
            jsonSerializerOptions.Converters.Add(converter);
        }

        if (config.UseDefaultConverters)
        {
            jsonSerializerOptions.Converters.Add(new TemplateConverter(registry));
        }

        var serializer = new Serializer(jsonSerializerOptions, jsonReaderOptions);
        return serializer;
    }
}
