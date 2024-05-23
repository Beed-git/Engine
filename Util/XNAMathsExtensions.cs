using Microsoft.Xna.Framework;

namespace Engine.Util;

public static class XNAMathsExtensions
{
    public static Vector2 ToVector2(this Point point) => new(point.X, point.Y);
    public static Point ToPoint(this Vector2 vector) => new((int)vector.X, (int)vector.Y);

    // Distances
    public static float Distance(this Point point, Point target)
    {
        var x2 = (point.X - target.X) * (point.X - target.X);
        var y2 = (point.Y - target.Y) * (point.Y - target.Y);
        return MathF.Sqrt(x2 + y2);
    }

    public static float Distance(this Vector2 vector, Vector2 target)
    {
        var x2 = (vector.X - target.X) * (vector.X - target.X);
        var y2 = (vector.Y - target.Y) * (vector.Y - target.Y);
        return MathF.Sqrt(x2 + y2);
    }
}
