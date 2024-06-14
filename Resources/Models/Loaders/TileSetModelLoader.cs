using Engine.Rendering;
using Engine.Resources.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Resources.Models.Loaders;

public class TileSetModelLoader
    : IModelResourceLoader<TileSet, TileSetModel>
{
    private readonly ResourceSystem _resources;

    public TileSetModelLoader(ResourceSystem resources)
    {
        _resources = resources;
    }

    public TileSet Load(TileSetModel model)
    {
        var sources = new List<Rectangle>();

        var name = new ResourceName(model.Texture);
        var texture = _resources.Get<Texture2D>(name);

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

        var tileset = new TileSet(texture, sources);
        return tileset;
    }
}