using Engine.Files;
using Engine.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Resources.Raw.Loaders;

public class TextureBinaryLoader
    : IRawResourceLoader<Texture2D>
{
    private readonly ILogger<TextureBinaryLoader> _logger;
    private readonly FileSystem _files;
    private readonly TextureSystem _textures;

    public TextureBinaryLoader(ILoggerFactory loggerFactory, FileSystem files, TextureSystem textures)
    {
        _logger = loggerFactory.CreateLogger<TextureBinaryLoader>();
        _files = files;
        _textures = textures;
    }

    public Texture2D Load(ResourceName name)
    {
        var path = $"{FileSystemSettings.AssetsFolder}{name}";
        if (!_files.Exists(path))
        {
            _logger.LogError("Failed to find texture resource with name '{}'", name);
            return _textures.MissingTexture;
        }

        using var stream = _files.OpenReadStream(path);
        var texture = Texture2D.FromStream(_textures.Graphics, stream);
        return texture;
    }
}
