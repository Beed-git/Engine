using Microsoft.Xna.Framework;

namespace Engine.Level;

public class TileChunk
{
    // 256x256
    internal const int TileBitCount = 8;

    internal const int TileWidth = 1 << TileBitCount;
    internal const int TileHeight = 1 << TileBitCount;

    internal const int MaxTileCount = TileWidth * TileHeight;

    private readonly Tile[] _tiles;

    internal TileChunk()
    {
        _tiles = new Tile[MaxTileCount];
    }

    public static int Width => TileWidth;
    public static int Height => TileHeight;

    public static int BitCount => TileBitCount;

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

        var tile = _tiles[index];
        return tile;
    }

    public Span<Tile> GetRow(int row)
    {
        if (row < 0 || row >= TileHeight)
        {
            return [];
        }

        var start = row * TileWidth;
        var tiles = new Span<Tile>(_tiles, start, TileWidth);
        return tiles;
    }

    public void SetTile(Point point, Tile tile)
    {
        if (!IsValid(point, out var index))
        {
            return;
        }

        _tiles[index] = tile;
    }

    public void Clear()
    {
        Array.Fill(_tiles, Tile.None);
    }

    private bool IsValid(Point point, out int index)
    {
        index = point.X + point.Y * TileWidth;
        var isValid = index >= 0 || index < _tiles.Length;
        return isValid;
    }
}