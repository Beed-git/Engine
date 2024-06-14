using Engine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Immutable;

namespace Engine.Rendering;

public class TileSet
{
    private readonly IList<Rectangle> _sources;
    private readonly Texture2D _texture;

    public TileSet(Texture2D texture, IEnumerable<Rectangle> sources)
    {
        _texture = texture;
        _sources = sources.ToImmutableList();
    }

    public int TileCount => _sources.Count;
    public Texture2D Texture => _texture;

    public Rectangle GetSource(int index)
    {
        if (index <= 0 || index > _sources.Count)
        {
            return Rectangle.Empty;
        }

        index -= 1;

        var source = _sources[index];
        return source;
    }
}
