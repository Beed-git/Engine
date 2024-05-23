namespace Engine.Level;

public static class TileChunkExtensions
{
    public static Tile GetTile(this TileChunk chunk, int x, int y)
    {
        return chunk.GetTile(new(x, y));
    }

    public static void SetTile(this TileChunk chunk, int x, int y, Tile tile)
    {
        chunk.SetTile(new (x, y), tile);
    }
}
