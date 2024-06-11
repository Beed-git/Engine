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
    private readonly Dictionary<ResourceName, Texture2D> _textures;
    private readonly FileSystem _files;

    internal TextureSystem(ILoggerFactory loggerFactory, GraphicsDevice graphics, FileSystem files)
    {
        Graphics = graphics;
        _logger = loggerFactory.CreateLogger<TextureSystem>();
        _files = files;
        _textures = [];

        WhiteSquare = new ("#blank", new Texture2D(graphics, 1, 1));
        WhiteSquare.Data.SetData([Color.White]);

        MissingTexture = new ("#missing", new Texture2D(graphics, 2, 2));
        MissingTexture.Data.SetData([Color.DarkViolet, Color.Black, Color.Black, Color.DarkViolet]);

        _textures.Add(WhiteSquare.Name, WhiteSquare);
        _textures.Add(MissingTexture.Name, MissingTexture);
    }

    public GraphicsDevice Graphics { get; private init; }
    public Resource<Texture2D> WhiteSquare { get; private init; }
    public Resource<Texture2D> MissingTexture { get; private init; }

    public Resource<Texture2D> GetTexture(ResourceName name)
    {
        if (_textures.TryGetValue(name, out var texture))
        {
            return new (name, texture);
        }

        var path = $"{FileSystemSettings.AssetsFolder}{name}";
        if (_files.TryOpenBinaryStream(path, "png", out var stream))
        {
            _logger.LogInformation("Loading texture '{}'", name);
            using (stream)
            {
                texture = Texture2D.FromStream(Graphics, stream);
                _textures.Add(name, texture);
                return new (name, texture);
            }
        }
        _logger.LogWarning("Attempted to get texture resource '{}' but resource was not found.", name);

        return MissingTexture;
    }

    public void Dispose()
    {
        foreach (var texture in _textures.Values)
        {
            texture.Dispose();
        }

        WhiteSquare.Data.Dispose();
        MissingTexture.Data.Dispose();
    }
}