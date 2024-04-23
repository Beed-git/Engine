using Engine.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Engine.Serialization.Converters;

public class TemplateConverter
    : JsonConverter<Template>
{
    private readonly ComponentRegistry _componentRegistry;

    private const string NameField = "name";
    private const string DescriptionField = "description";
    private const string ComponentsField = "components";

    public TemplateConverter(ComponentRegistry componentRegistry)
    {
        _componentRegistry = componentRegistry;
    }

    public override Template? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        reader.Expect(JsonTokenType.StartObject);

        var name = string.Empty;
        var description = string.Empty;
        var components = new List<object>();

        while (reader.Read())
        {
            if (reader.Is(JsonTokenType.EndObject))
            {
                break;
            }

            reader.Expect(JsonTokenType.PropertyName);
            var property = reader.GetNonEmptyString().Trim();
            reader.Read();

            if (string.IsNullOrWhiteSpace(property)) 
            {
                throw new JsonException("Expected property but got an empty name.");
            }

            if (property.Equals(NameField, StringComparison.OrdinalIgnoreCase))
            {
                reader.Expect(JsonTokenType.String);
                name = reader.GetNonEmptyString();
            }
            else if (property.Equals(DescriptionField, StringComparison.OrdinalIgnoreCase))
            {
                reader.Expect(JsonTokenType.String);
                description = reader.GetNonEmptyString();
            }
            else if (property.Equals(ComponentsField, StringComparison.OrdinalIgnoreCase))
            {
                ReadComponents(ref reader, components, options);
            }
            else
            {
                reader.ThrowUnexpectedPropertyName();
            }
        }

        reader.Expect(JsonTokenType.EndObject);

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new JsonException($"Expected field '{NameField}' was not found.");
        }
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new JsonException($"Expected field '{NameField}' was not found.");
        }

        var template = new Template(name, description, components);
        return template;
    }

    private void ReadComponents(ref Utf8JsonReader reader, List<object> components, JsonSerializerOptions options)
    {
        reader.Expect(JsonTokenType.StartObject);
        while (reader.Read())
        {
            if (reader.Is(JsonTokenType.EndObject))
            {
                break;
            }

            reader.Expect(JsonTokenType.PropertyName);
            var name = reader.GetNonEmptyString();
            if (!_componentRegistry.TryGetType(name, out var type))
            {
                throw new JsonException($"Component with name '{name}' is not registered.");
            }

            // Won't be null as component has to be a struct.
            var component = JsonSerializer.Deserialize(ref reader, type, options)!;
            components.Add(component);
        }

        reader.Expect(JsonTokenType.EndObject);
    }

    public override void Write(Utf8JsonWriter writer, Template value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();  
        writer.WriteString(NameField, value.Name);
        writer.WriteString(DescriptionField, value.Description);
        writer.WritePropertyName(ComponentsField);
        writer.WriteStartObject();
        foreach (var component in value.Components) 
        {
            var type = component.GetType();
            if (!_componentRegistry.TryGetName(type, out var name))
            {
                throw new JsonException($"Component of type '{type}' is not registered.");
            }

            writer.WritePropertyName(name);
            JsonSerializer.Serialize(writer, component, type, options);
        }
        writer.WriteEndObject();
        writer.WriteEndObject();
    }
}
