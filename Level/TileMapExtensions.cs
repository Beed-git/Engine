using Microsoft.Xna.Framework;
using System.Diagnostics.CodeAnalysis;

namespace Engine.Level;

public static class TileMapExtensions
{
    // Get Tile

    public static Tile GetTile(this TileMap map, Point position)
    {
        return GetTile(map, position.X, position.Y);
    }

    public static Tile GetTile(this TileMap map, int x, int y)
    {
        var chunkX = x >> TileChunk.TileBitCount;
        var chunkY = y >> TileChunk.TileBitCount;

        var chunk = map.GetChunk(new (chunkX, chunkY));

        var tileX = x & (TileChunk.TileWidth - 1);
        var tileY = y & (TileChunk.TileHeight - 1);


        var tile = chunk.Chunk.GetTile(new Point (tileX, tileY));
        return tile;
    }

    public static bool GetTileIfLoaded(this TileMap map, Point position, out Tile tile)
    {
        return GetTileIfLoaded(map, position.X, position.Y, out tile);
    }

    public static bool GetTileIfLoaded(this TileMap map, int x, int y, out Tile tile)
    {
        tile = Tile.None;

        var chunkX = x >> TileChunk.TileBitCount;
        var chunkY = y >> TileChunk.TileBitCount;

        if (!map.GetChunkIfLoaded(chunkX, chunkY, out var chunk))
        {
            return false;
        }

        var tileX = x & (TileChunk.TileWidth - 1);
        var tileY = y & (TileChunk.TileHeight - 1);

        tile = chunk.GetTile(new Point(tileX, tileY));
        return true;
    }

    // Set Tile

    public static void SetTile(this TileMap map, Point position, Tile tile)
    {
        SetTile(map, position.X, position.Y, tile);
    }

    public static void SetTile(this TileMap map, int x, int y, Tile tile)
    {
        var chunkX = x >> TileChunk.TileBitCount;
        var chunkY = y >> TileChunk.TileBitCount;

        var chunk = map.GetChunk(new(chunkX, chunkY));

        var tileX = x & (TileChunk.TileWidth - 1);
        var tileY = y & (TileChunk.TileHeight - 1);

        chunk.Chunk.SetTile(new Point(tileX, tileY), tile);
    }

    public static bool SetTileIfLoaded(this TileMap map, Point position, Tile tile)
    {
        return SetTileIfLoaded(map, position.X, position.Y, tile);
    }

    public static bool SetTileIfLoaded(this TileMap map, int x, int y, Tile tile)
    {
        var chunkX = x >> TileChunk.TileBitCount;
        var chunkY = y >> TileChunk.TileBitCount;

        if (!map.GetChunkIfLoaded(new(chunkX, chunkY), out var chunk))
        {
            return false;
        }

        var tileX = x & (TileChunk.TileWidth - 1);
        var tileY = y & (TileChunk.TileHeight - 1);

        chunk.SetTile(new Point(tileX, tileY), tile);
        return true;
    }

    // Chunk
    public static TileChunkMetadata GetChunk(this TileMap map, int x, int y)
    {
        return map.GetChunk(new Point(x, y));
    }

    public static bool GetChunkIfLoaded(this TileMap map, int x, int y, [MaybeNullWhen(false)] out TileChunk chunk)
    {
        return map.GetChunkIfLoaded(new Point(x, y), out chunk);
    }

    public static TileChunkMetadata? DeleteChunk(this TileMap map, int x, int y)
    {
        return map.DeleteChunk(new Point(x, y));
    }
}
