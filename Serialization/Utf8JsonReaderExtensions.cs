using System.Text.Json;

namespace Engine.Serialization;

public static class Utf8JsonReaderExtensions
{
    public static void Expect(this Utf8JsonReader reader, JsonTokenType type)
    {
        if (reader.TokenType != type) 
        {
            throw new JsonException($"Expected token of type '{type}' but got token '{reader.TokenType}' with value '{reader.GetString()}'");
        }
    }

    public static bool Is(this Utf8JsonReader reader, JsonTokenType type)
    {
        return reader.TokenType == type;
    }

    public static string GetNonEmptyString(this Utf8JsonReader reader)
    {
        var text = reader.GetString();
        if (text is null)
        {
            throw new JsonException("Expected a string but got null.");
        }
        else if (string.IsNullOrWhiteSpace(text))
        {
            throw new JsonException("Expected a string of text but got whitespace.");
        }
        return text;
    }

    public static void ThrowUnexpectedPropertyName(this Utf8JsonReader reader)
    {
        Expect(reader, JsonTokenType.PropertyName);
        var property = reader.GetString();
        throw new JsonException($"Unexpected property name '{property}'");
    }
}
