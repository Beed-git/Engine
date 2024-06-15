using Engine.Level;
using Microsoft.Xna.Framework;

namespace Engine.ECS.Components;

[Component]
public partial struct PositionComponentOld
{
    public float X;
    public float Y;

    public PositionComponentOld(float x, float y)
    {
        X = x;
        Y = y;
    }

    public readonly bool IsZero => X == 0 && Y == 0;

    public readonly override bool Equals(object? obj)
    {
        return obj is PositionComponentOld component &&
               X == component.X &&
               Y == component.Y;
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static bool operator ==(PositionComponentOld left, PositionComponentOld right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PositionComponentOld left, PositionComponentOld right)
    {
        return !(left == right);
    }

    public static explicit operator PositionComponentOld(Vector2 position) => new (position.X, position.Y);
    public static explicit operator PositionComponentOld(Point position) => new (position.X, position.Y);

    public static explicit operator Vector2(PositionComponentOld position) => new (position.X, position.Y);
    public static explicit operator Point(PositionComponentOld position) => new ((int)position.X, (int)position.Y);
}
