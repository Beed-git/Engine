using Arch.Core;
using Engine.Rendering;

namespace Engine.Level;

public class Scene
{
    public Scene(string name, TileMap tiles)
        : this(name, tiles, World.Create(), new Camera2D())
    {
    }

    public Scene(string name, TileMap tiles, World entities, Camera2D camera)
    {
        Name = name;
        Tiles = tiles;
        Entities = entities;
        Camera = camera;
    }

    public string Name { get; private init; }
    public TileMap Tiles { get; private init; }
    public World Entities { get; private init; }
    public Camera2D Camera { get; private init; }
}
