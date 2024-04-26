﻿using Microsoft.Xna.Framework;

namespace Engine.ECS.Components;

[Component]
public partial struct PositionComponent
{
    public float X;
    public float Y;

    public PositionComponent(float x, float y)
    {
        X = x;
        Y = y;
    }

    public readonly bool IsEmpty => X == 0 && Y == 0;

    public readonly override bool Equals(object? obj)
    {
        return obj is PositionComponent component &&
               X == component.X &&
               Y == component.Y;
    }

    public readonly override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public static bool operator ==(PositionComponent left, PositionComponent right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(PositionComponent left, PositionComponent right)
    {
        return !(left == right);
    }

    public static explicit operator PositionComponent(Vector2 position) => new (position.X, position.Y);
    public static explicit operator PositionComponent(Point position) => new (position.X, position.Y);

    public static explicit operator Vector2(PositionComponent position) => new (position.X, position.Y);
    public static explicit operator Point(PositionComponent position) => new ((int)position.X, (int)position.Y);
}