using Arch.Core;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Engine.Serialization.Converters;

public class EntityReferenceConverter
    : JsonConverter<EntityReference>
{
    public override EntityReference Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, EntityReference value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
