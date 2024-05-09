using Microsoft.Xna.Framework;

namespace Engine.Rendering;

public class TileSheet
{
    private readonly int _tileCountX;
    private readonly int _tileCountY;
    private readonly int _totalTileCount;

    public TileSheet(int tileWidth, int tileHeight, int width, int height)
    {
        TileWidth = tileWidth;
        TileHeight = tileHeight;
        Width = width;
        Height = height;

        _tileCountX = tileWidth <= 0 ? 1 : Width / tileWidth;
        _tileCountY = tileHeight <= 0 ? 1 : Height / tileHeight;
        _totalTileCount = _tileCountX * _tileCountY;
    }

    public int TileWidth { get; private init; }
    public int TileHeight { get; private init; }
    public int Width { get; private init; }
    public int Height { get; private init; }

    public static TileSheet Empty => new(0, 0, 0, 0);

    public Rectangle GetSource(int id)
    {
        if (id < 0 || id > _totalTileCount)
        {
            return Rectangle.Empty;
        }

        var x = id % _tileCountX;
        var y = id / _tileCountY;

        var bounds = GetSource(x, y);
        return bounds;
    }

    public Rectangle GetSource(int x, int y)
    {
        var bounds = new Rectangle(
            x * TileWidth,
            y * TileHeight,
            TileWidth,
            TileHeight);
        return bounds;
    }
}
