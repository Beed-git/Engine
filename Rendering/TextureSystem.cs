using Engine.Files;
using Engine.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Rendering;

public class TextureSystem
    : IDisposable
{
    private readonly ILogger _logger;
    private readonly Dictionary<Resource, Texture2D> _textures;
    private readonly FileSystem _files;

    public TextureSystem(ILoggerFactory loggerFactory, GraphicsDevice graphics, FileSystem files)
    {
        Graphics = graphics;
        _logger = loggerFactory.CreateLogger<TextureSystem>();
        _files = files;
        _textures = [];

        WhiteSquare = new Texture2D(graphics, 1, 1);
        WhiteSquare.SetData([Color.White]);
    }

    public GraphicsDevice Graphics { get; private init; }
    public Texture2D WhiteSquare { get; private init; }

    public Texture2D Get(Resource resource)
    {
        if (_textures.TryGetValue(resource, out var texture))
        {
            return texture;
        }

        _logger.LogWarning("Attempted to get texture resource '{}' but resource was not found.", resource);
        return WhiteSquare;
    }

    public void Dispose()
    {
        foreach (var texture in _textures.Values)
        {
            texture.Dispose();
        }

        WhiteSquare.Dispose();
    }
}