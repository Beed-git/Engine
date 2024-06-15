using Microsoft.Xna.Framework;

namespace Engine.ECS.Components;

[Component]
public struct SpriteComponentOld
{
    public Color Color;

    public SpriteComponentOld(Color color)
    {
        Color = color;
    }
}
