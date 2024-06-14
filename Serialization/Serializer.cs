using System.Text;
using System.Text.Json;

namespace Engine.Serialization;

public class Serializer
{
    private readonly JsonSerializerOptions _options;
    private readonly JsonReaderOptions _readerOptions; 

    public Serializer(JsonSerializerOptions options, JsonReaderOptions readerOptions)
    {
        _options = options;
        _readerOptions = readerOptions;
    }

    public JsonSerializerOptions SerializerOptions => _options;
    public JsonReaderOptions ReaderOptions => _readerOptions;

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

    public T Deserialize<T>(ReadOnlySpan<byte> json)
    {
        if (json.StartsWith(Encoding.UTF8.Preamble))
        {
            json = json[Encoding.UTF8.Preamble.Length..];
        }

        var obj = JsonSerializer.Deserialize<T>(json, _options)
            ?? throw new Exception($"Deserialization failed, expected an object but got null."); ;
        return obj;
    }

    internal T Deserialize<T>(ref Utf8JsonReader reader)
    {
        var obj = JsonSerializer.Deserialize<T>(ref reader, _options)
            ?? throw new Exception($"Deserialization failed, expected an object but got null.");;
        return obj;
    }

    internal Utf8JsonReader CreateReader(ReadOnlySpan<byte> json)
    {
        if (json.StartsWith(Encoding.UTF8.Preamble))
        {
            json = json[Encoding.UTF8.Preamble.Length..];
        }

        var reader = new Utf8JsonReader(json, _readerOptions);
        return reader;
    }
}
