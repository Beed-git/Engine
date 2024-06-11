using Arch.Core;
using Engine.Rendering;
using Engine.Resources;

namespace Engine.Level;

public class Scene
{
    internal Scene(ResourceName name, World entities, Camera2D camera)
    {
        Name = name;
        Entities = entities;
        Camera = camera;
    }

    public ResourceName Name { get; private init; }
    public World Entities { get; private init; }
    public Camera2D Camera { get; private init; }
}
