using Microsoft.Xna.Framework;

namespace Engine.ECS.Components;

[Component]
public struct SpriteComponent
{
    public Color Color;

    public SpriteComponent(Color color)
    {
        Color = color;
    }
}
