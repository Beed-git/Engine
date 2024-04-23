using System.Text.Json;
using System.Text.Json.Serialization;

namespace Engine.Serialization;

public class Serializer
{
    private readonly JsonSerializerOptions _options;

    public Serializer(JsonSerializerOptions options)
    {
        _options = options;
    }

    public string Serialize<T>(T data)
    {
        ArgumentNullException.ThrowIfNull(data);
        var serialized = JsonSerializer.Serialize<T>(data, _options)
            ?? throw new Exception($"Serialization failed, as object was null."); ;
        return serialized;
    }

    public void Serialize(Utf8JsonWriter writer, Type type, object data)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(data);

        JsonSerializer.Serialize(writer, data, type, _options);
    }

    public T Deserialize<T>(string json)
    {
        ArgumentNullException.ThrowIfNull(json);
        var obj = JsonSerializer.Deserialize<T>(json, _options)
            ?? throw new Exception($"Deserialization failed, expected an object but got null.");;
        return obj;
    }
}
