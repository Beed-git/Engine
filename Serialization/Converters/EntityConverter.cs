using Arch.Core;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Engine.Serialization.Converters;

public class EntityConverter
    : JsonConverter<Entity>
{
    private readonly EntityRegistry _entities;

    public override Entity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();

        //reader.Read();
        //reader.Expect(JsonTokenType.String);
    }

    public override void Write(Utf8JsonWriter writer, Entity value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
