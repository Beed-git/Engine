using Microsoft.Xna.Framework;

namespace Engine.Maths.Hexagons;

public class HexGridLayout
{
    private float _size;

    public HexGridLayout(int size)
    {
        _size = size;
    }

    public float Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _size = value;
                CalculateDimensions();
            }
        }
    }

    public Vector2 HexagonDimensions { get; private set; }

    private void CalculateDimensions()
    {
        var width = 2 * _size;
        var height = MathF.Sqrt(3) * _size;
        HexagonDimensions = new Vector2(width, height);
    }

    public AxialCoordinate WorldPositionToHex(Vector2 position)
    {
        var q = 2f / 3 * position.X;
        var r = -1f / 3 * position.X + MathF.Sqrt(3) / 3 * position.Y;

        var coord = AxialCoordinate.Round(q, r);
        return coord;
    }
}