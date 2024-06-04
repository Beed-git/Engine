using Microsoft.Xna.Framework;

namespace Engine.Maths;

public struct RectangleF
{
    public float X;
    public float Y;
    public float Width;
    public float Height;

    public RectangleF(float x, float y, float width, float height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public static RectangleF Empty => new(0, 0, 0, 0);

    public static explicit operator RectangleF(Rectangle rectangle) => new(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
    public static explicit operator Rectangle(RectangleF rectangle) => new((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
}
