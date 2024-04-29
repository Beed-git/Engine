using Arch.Core;
using Engine.Rendering;
using Engine.Resources;

namespace Engine.Level;

public class Scene
{
    public Scene(Resource name, TileMap tiles)
        : this(name, tiles, World.Create(), new Camera2D())
    {
    }

    public Scene(Resource name, TileMap tiles, Camera2D camera)
        : this(name, tiles, World.Create(), camera)
    { 
    }

    public Scene(Resource name, TileMap tiles, World entities, Camera2D camera)
    {
        Name = name;
        Tiles = tiles;
        Entities = entities;
        Camera = camera;
    }

    public static Scene Empty => new("empty", new TileMap(1, 1), World.Create(), new Camera2D());

    public Resource Name { get; private init; }
    public TileMap Tiles { get; private init; }
    public World Entities { get; private init; }
    public Camera2D Camera { get; private init; }
}
