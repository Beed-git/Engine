using Microsoft.Xna.Framework;

namespace Engine.Maths.Hexagons;

public readonly struct AxialCoordinate
{
    public readonly int Q;
    public readonly int R;

    public AxialCoordinate(int q, int r)
    {
        Q = q;
        R = r;
    }

    public readonly int S => -Q - R;

    public static AxialCoordinate Zero => new(0, 0);

    public static AxialCoordinate Round(float q, float r)
    {
        var s = -q - r;

        var qRounded = (int)MathF.Round(q);
        var rRounded = (int)MathF.Round(r);
        var sRounded = (int)MathF.Round(s);

        var qDifference = MathF.Abs(qRounded - q);
        var rDifference = MathF.Abs(rRounded - r);
        var sDifference = MathF.Abs(sRounded - s);

        if (qDifference > rDifference && qDifference > sDifference)
        {
            qRounded = -rRounded - sRounded;
        }
        else if (rDifference > sDifference)
        {
            rRounded = -qRounded - sRounded;
        }

        var coordinate = new AxialCoordinate(qRounded, rRounded);
        return coordinate;
    }

    public static int Distance(AxialCoordinate a, AxialCoordinate b)
    {
        var difference = a - b;
        var distance = (Math.Abs(difference.Q)
                      + Math.Abs(difference.Q + difference.R)
                      + Math.Abs(difference.R)) / 2;
        return distance;
    }

    public readonly int Distance(AxialCoordinate target)
    {
        return Distance(this, target);
    }

    public readonly override string ToString()
    {
        return $"({Q},{R},{S})";
    }

    /// <summary>
    /// Converts from an Odd-q position.
    /// </summary>
    /// <param name="position"></param>
    public static explicit operator AxialCoordinate(Point position) =>
        new(position.X,
            position.Y - (position.X - (position.X & 1)) / 2);

    public static explicit operator Point(AxialCoordinate coordinate) =>
        new(coordinate.Q,
            coordinate.R + (coordinate.Q - (coordinate.Q & 1)) / 2);

    public static AxialCoordinate operator +(AxialCoordinate left, AxialCoordinate right) => new(left.Q + right.Q, left.R + right.R);
    public static AxialCoordinate operator -(AxialCoordinate left, AxialCoordinate right) => new(left.Q - right.Q, left.R - right.R);
}
