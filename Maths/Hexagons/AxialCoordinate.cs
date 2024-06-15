using Microsoft.Xna.Framework;
using System.Diagnostics.CodeAnalysis;

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

    public static IEnumerable<AxialCoordinate> HexesInLine(AxialCoordinate start, AxialCoordinate end)
    {
        const float nudgeQ = 1e-6f;
        const float nudgeR = 2e-6f;

        var distance = AxialCoordinate.Distance(start, end);

        for (int i = 0; i < distance; i++)
        {
            var amount = (1.0f / distance) * i;
            var qLerp = float.Lerp(start.Q + nudgeQ, end.Q + nudgeQ, amount);
            var rLerp = float.Lerp(start.R + nudgeR, end.R + nudgeR, amount);

            var coordinate = Round(qLerp, rLerp);
            yield return coordinate;
        }
    }

    public readonly override string ToString()
    {
        return $"({Q},{R},{S})";
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
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

    public static bool operator ==(AxialCoordinate left, AxialCoordinate right)
    {
        return left.Q == right.Q
            && left.R == right.R;
    }

    public static bool operator !=(AxialCoordinate left, AxialCoordinate right)
    {
        return !(left == right);
    }
}
