using Engine.Data;
using Engine.Files;
using Engine.Rendering;
using FontStashSharp;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Configuration;

public class Dependencies
    : IDisposable
{
    internal Dependencies(
        ILoggerFactory loggerFactory,
        Database database,
        FileSystem fileSystem,
        FontSystem fonts,
        Screen screen,
        TextureSystem textures)
    {
        LoggerFactory = loggerFactory;
        Database = database;
        FileSystem = fileSystem;
        Fonts = fonts;
        Screen = screen;
        Textures = textures;
    }

    public ILoggerFactory LoggerFactory { get; private init; }
    public Database Database { get; private init; }
    public FileSystem FileSystem { get; private init; }
    public FontSystem Fonts { get; private init; }
    public Screen Screen { get; private init; }
    public TextureSystem Textures { get; private init; }

    // NOTE: Leave graphics to be disposed by MainGame.
    public void Dispose()
    {
        Fonts.Dispose();
    }
}
