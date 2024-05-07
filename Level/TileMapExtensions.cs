using Microsoft.Xna.Framework;

namespace Engine.Level;

public static class TileMapExtensions
{
    public static Tile GetTile(this TileMap map, Point position)
    {
        var chunkPosition = new Point(
            position.X / TileChunk.Width,
            position.Y / TileChunk.Height);

        if (!map.GetChunkIfExists(chunkPosition, out var chunk))
        {
            return Tile.None;
        }

        var tilePosition = new Point(
            position.X % TileChunk.Width,
            position.Y / TileChunk.Height);

        var tile = chunk.GetTile(tilePosition);
        return tile;
    }

    public static void SetTile(this TileMap map, Point position, Tile tile)
    {
        var chunkPosition = new Point(
            position.X / TileChunk.Width,
            position.Y / TileChunk.Height);

        if (!map.GetChunkIfExists(chunkPosition, out var chunk))
        {
            return;
        }

        var tilePosition = new Point(
            position.X % TileChunk.Width,
            position.Y / TileChunk.Height);

        chunk.SetTile(tilePosition, tile);
    }
}
