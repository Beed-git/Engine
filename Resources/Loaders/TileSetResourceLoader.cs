using Engine.Rendering;
using Engine.Resources.Models;
using Engine.Serialization;
using Microsoft.Xna.Framework;
using System.Text.Json;

namespace Engine.Resources.Loaders;

internal class TileSetResourceLoader
    : IResourceLoader<TileSet>
{
    private readonly Serializer _serializer;
    private readonly TextureSystem _textures;

    public TileSetResourceLoader(Serializer serializer, TextureSystem textures)
    {
        _serializer = serializer;
        _textures = textures;
    }

    public TileSet Load(ref Utf8JsonReader reader)
    {
        var model = _serializer.Deserialize<TileSetModel>(ref reader);

        var sources = new List<Rectangle>();

        var name = new ResourceName(model.Texture);
        var resource = _textures.GetTexture(name);
        
        foreach (var auto in model.Auto)
        {
            var width = (auto.Bounds.X + auto.Bounds.Width) / auto.Tiles.Width;
            var height = (auto.Bounds.Y + auto.Bounds.Height) / auto.Tiles.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var rectangle = new Rectangle(
                        auto.Bounds.X + x * auto.Tiles.Width,
                        auto.Bounds.Y + y * auto.Tiles.Height,
                        auto.Tiles.Width,
                        auto.Tiles.Height);

                    sources.Add(rectangle);
                }
            }
        }

        var tileset = new TileSet(resource, sources);
        return tileset;
    }
}