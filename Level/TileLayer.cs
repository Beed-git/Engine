using Engine.Rendering;

namespace Engine.Level;

public class TileLayer
{
    public TileLayer(TileSet tileSet, int size)
    {
        TileSet = tileSet;
        Tiles = new Tile[size];
    }

    public TileSet TileSet { get; private init; }
    public Tile[] Tiles { get; private init; }
}
