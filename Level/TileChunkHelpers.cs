using Engine.ECS.Components;
using Microsoft.Xna.Framework;

namespace Engine.Level;

public static class TileChunkHelpers
{
    public static Point TilePositionToChunkPosition(Point position)
    {
        return new Point(
            position.X >> TileChunk.TileBitCount,
            position.Y >> TileChunk.TileBitCount);
    }

    public static Point TilePositionToChunkPosition(Vector2 position)
    {
        return new Point(
            (int)position.X >> TileChunk.TileBitCount,
            (int)position.Y >> TileChunk.TileBitCount);
    }

    public static Point TilePositionToChunkPosition(PositionComponent position)
    {
        return new Point(
            (int)position.X >> TileChunk.TileBitCount,
            (int)position.Y >> TileChunk.TileBitCount);
    }
}