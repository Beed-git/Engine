using Microsoft.Xna.Framework;

namespace Engine.Util;

public static class XNAMathsExtensions
{
    public static Vector2 ToVector2(this Point point) => new(point.X, point.Y);
    public static Point ToPoint(this Vector2 vector) => new((int)vector.X, (int)vector.Y);
}
