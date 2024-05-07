using Microsoft.Xna.Framework;

namespace Engine.Level;

public class TileChunk
{
    private const int TileWidth = 256;
    private const int TileHeight = 256;

    private const int MaxTileCount = TileWidth * TileHeight;

    internal TileChunk()
    {
        Tiles = new Tile[MaxTileCount];
    }

    public static int Width => TileWidth;
    public static int Height => TileHeight;

    public Tile[] Tiles { get; private init; }

    public bool IsValid(Point point)
    {
        return IsValid(point, out _);
    }

    public Tile GetTile(Point point)
    {
        if (!IsValid(point, out var index))
        {
            return Tile.None;
        } 

        var tile = Tiles[index];
        return tile;
    }

    public void SetTile(Point point, Tile tile)
    {
        if (!IsValid(point, out var index))
        {
            return;
        }

        Tiles[index] = tile;
    }

    public void Clear()
    {
        Array.Fill(Tiles, Tile.None);
    }

    private bool IsValid(Point point, out int index)
    {
        index = point.X + point.Y * TileWidth;
        var isValid = index >= 0 || index < Tiles.Length;
        return isValid;
    }
}