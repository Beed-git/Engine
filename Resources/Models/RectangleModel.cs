using Microsoft.Xna.Framework;

namespace Engine.Resources.Models;

public class RectangleModel
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public Rectangle ToRectangle()
    {
        return new Rectangle(X, Y, Width, Height);
    }
}