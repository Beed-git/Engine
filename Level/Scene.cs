using Arch.Core;
using Engine.Rendering;
using Engine.Resources;

namespace Engine.Level;

public class Scene
{
    internal Scene(Resource name, TileMap map, World entities, Camera2D camera)
    {
        Name = name;
        Map = map;
        Entities = entities;
        Camera = camera;
    }

    public Resource Name { get; private init; }
    public TileMap Map { get; private init; }
    public World Entities { get; private init; }
    public Camera2D Camera { get; private init; }
}
