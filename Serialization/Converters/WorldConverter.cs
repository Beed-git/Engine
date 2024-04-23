using Arch.Core;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Engine.Serialization.Converters;

public class WorldConverter
    : JsonConverter<World>
{
    private readonly QueryDescription _query;
    private readonly ComponentRegistry _components;

    public WorldConverter(ComponentRegistry components)
    {
        _query = new QueryDescription();
        _components = components;
    }

    public override World? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, World world, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
        //writer.WriteStartArray();

        //world.Query(in _query, (entity) =>
        //{
        //    writer.WriteStartObject();
            
        //    var components = entity.GetAllComponents();
        //    foreach (var component in components)
        //    {

        //    }

        //    writer.WriteEndObject();
        //});

        //writer.WriteEndArray();
    }
}
