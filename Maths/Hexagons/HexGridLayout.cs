using Microsoft.Xna.Framework;

namespace Engine.Maths.Hexagons;

public class HexGridLayout
{
    private Point _mapSize;

    public HexGridLayout(Point mapSize)
    {
        MapSize = mapSize;
    }

    public Point MapSize
    {
        get => _mapSize;
        set
        {
            if (value.X <= 0 || value.Y <= 0)
            {
                throw new NotImplementedException("Infinite hexagonal maps are not yet supported.");
            }
            _mapSize = value;
        }
    }

    public bool IsOnMap(AxialCoordinate coordinate)
    {
        var point = (Point)coordinate;
        return point.X >= 0 && point.X < _mapSize.X
            && point.Y >= 0 && point.Y < _mapSize.Y;
    }

    public AxialCoordinate WorldPositionToHex(Vector2 position)
    {
        var q = 2f / 3 * position.X;
        var r = -1f / 3 * position.X + MathF.Sqrt(3) / 3 * position.Y;

        var coord = AxialCoordinate.Round(q, r);
        return coord;
    }

    public IEnumerable<AxialCoordinate> HexesInRange(AxialCoordinate center, int range)
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