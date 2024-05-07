using Microsoft.Xna.Framework;

namespace Engine.Level;

public class EmptyChunkGenerator
    : IChunkGenerator
{
    public void Generate(Point position, TileChunk chunk)
    {
    }
}
