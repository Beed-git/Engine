using Microsoft.Xna.Framework;

namespace Engine.Level;

public class TileMap
{
    public TileMap(int width, int height)
    {
        Width = Math.Max(1, width);
        Height = Math.Max(1, height);
        Tiles = new Tile[Width * Height];
    }

    public int Width { get; private init; }
    public int Height { get; private init; }
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

    private bool IsValid(Point point, out int index)
    {
        index = point.X + point.Y * Width;
        var isValid = index >= 0 || index < Tiles.Length;
        return isValid;
    }
}
