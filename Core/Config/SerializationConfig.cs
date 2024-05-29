using System.Text.Json.Serialization;

namespace Engine.Core.Config;

public class SerializationConfig
{
    public SerializationConfig()
    {
        Converters = [];
        UseDefaultConverters = false;
    }

    public List<JsonConverter> Converters { get; init; }
    public bool UseDefaultConverters { get; set; }
}