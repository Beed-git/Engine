using Engine.Level;
using Microsoft.Xna.Framework;

namespace Engine.Maths.Hexagons;

public static class TileMapHexExtensions
{
    public static bool IsOnMap(this TileMap tileMap, AxialCoordinate coordinate)
    {
        var point = (Point)coordinate;
        return point.X >= 0 && point.X < tileMap.Width
            && point.Y >= 0 && point.Y < tileMap.Height;
    }

    public static AxialCoordinate WorldPositionToHex(this TileMap tileMap, Vector2 position)
    {
        var q = 2f / 3 * position.X;
        var r = -1f / 3 * position.X + MathF.Sqrt(3) / 3 * position.Y;

        var coord = AxialCoordinate.Round(q, r);
        return coord;
    }

    public static IEnumerable<AxialCoordinate> HexesInRange(this TileMap tileMap, AxialCoordinate center, int range)
    {
        if (range < 0)
        {
            return [];
        }

        if (range == 0)
        {
            return [center];
        }

        var results = new HashSet<AxialCoordinate>();
        for (int q = -range; q <= range; q++)
        {
            for (int r = Math.Max(-range, -q - range); r <= Math.Min(range, -q + range); r++)
            {
                var coordinate = new AxialCoordinate(q, r);
                results.Add(center + coordinate);
            }
        }
        return results;
    }
}