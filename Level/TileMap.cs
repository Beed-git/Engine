using Microsoft.Xna.Framework;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Engine.Level;

public class TileMap
    : IEnumerable<KeyValuePair<Point, TileChunkMetadata>>
{
    private readonly Dictionary<Point, TileChunkMetadata> _chunks;
    private readonly IChunkGenerator _generator;

    public TileMap(IChunkGenerator generator)
    {
        _generator = generator;
        _chunks = [];
    }

    public TileChunkMetadata GetChunk(Point point)
    {
        if (!_chunks.TryGetValue(point, out var metadata))
        {
            var chunk = new TileChunk();
            _generator.Generate(point, chunk);

            metadata = new TileChunkMetadata(chunk, TileChunkSource.Generated);
            _chunks.Add(point, metadata);
        }

        return metadata;
    }

    internal bool GetChunkIfExists(Point point, [MaybeNullWhen(false)] out TileChunk chunk)
    {
        if (!_chunks.TryGetValue(point, out var metadata))
        {
            chunk = null;
            return false;
        }

        chunk = metadata.Chunk;
        return true;
    }


    public TileChunkMetadata? DeleteChunk(Point point)
    {
        if (!_chunks.Remove(point, out var metadata))
        {
            return null;
        }

        return metadata;
    }

    public IEnumerator<KeyValuePair<Point, TileChunkMetadata>> GetEnumerator()
    {
        return _chunks.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
