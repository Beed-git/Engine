using System.Text.Json.Serialization;

namespace Engine.Core.Config;

public class SerializationConfig
{
    private readonly List<JsonConverter> _converters;

    public SerializationConfig()
    {
        _converters = [];
        UseDefaultConverters = false;
    }

    public IEnumerable<JsonConverter> Converters => _converters;
    public bool UseDefaultConverters { get; private set; }

    public void AddConverter<T>(JsonConverter<T> converter)
    {
        _converters.Add(converter);
    }

    public void AddDefaultConverters()
    {
        UseDefaultConverters = true;
    }
}