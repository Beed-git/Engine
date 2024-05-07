using Microsoft.Xna.Framework;

namespace Engine.Level;

public interface IChunkGenerator
{
    public void Generate(Point position, TileChunk chunk);
}
