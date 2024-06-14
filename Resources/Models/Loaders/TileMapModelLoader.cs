using Engine.Level;
using Engine.Rendering;
using Engine.Resources.Models;

namespace Engine.Resources.Models.Loaders;

public class TileMapModelLoader
    : IModelResourceLoader<TileMap, TileMapModel>
{
    private readonly ResourceSystem _resources;

    public TileMapModelLoader(ResourceSystem resources)
    {
        _resources = resources;
    }

    public TileMap Load(TileMapModel model)
    {
        var tilemap = new TileMap(model.Size.Width, model.Size.Height);
        var size = tilemap.Width * tilemap.Height;

        foreach (var layerModel in model.Layers)
        {
            var tileset = _resources.Get<TileSet>(layerModel.TileSet);
            var layer = new TileLayer(tileset, size);

            SetTiles(layer, layerModel.Tiles);

            tilemap.AddLayer(layer);
        }

        return tilemap;
    }

    private void SetTiles(TileLayer layer, IEnumerable<string> tiles)
    {
        // TODO: Very inefficient.
        var index = 0;
        foreach (var row in tiles)
        {
            var nums = row.Split(',');
            for (int i = 0; i < nums.Length; i++)
            {
                var tile = ushort.Parse(nums[i]);
                layer.Tiles[index] = new Tile(tile);
                index++;
            }
        }
    }
}
