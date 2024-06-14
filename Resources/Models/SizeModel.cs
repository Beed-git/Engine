using Microsoft.Xna.Framework;

namespace Engine.Resources.Models;

public class SizeModel
{
    public int Height { get; set; }
    public int Width { get; set; }

    public Vector2 ToVector2()
    {
        return new Vector2(Width, Height);
    }

    public Point ToPoint()
    {
        return new Point(Width, Height);
    }
}
