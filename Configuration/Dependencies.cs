using Engine.Data;
using Engine.ECS;
using Engine.Files;
using Engine.Level;
using Engine.Rendering;
using FontStashSharp;
using Microsoft.Extensions.Logging;

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
        StageManager stages,
        TextureSystem textures)
    {
        LoggerFactory = loggerFactory;
        Database = database;
        FileSystem = fileSystem;
        Fonts = fonts;
        Screen = screen;
        Stages = stages;
        Textures = textures;
    }

    public SceneManager SceneManager { get; internal set; }
    public EventSystem Events { get; internal set; }

    public ILoggerFactory LoggerFactory { get; private init; }
    public Database Database { get; private init; }
    public FileSystem FileSystem { get; private init; }
    public FontSystem Fonts { get; private init; }
    public Screen Screen { get; private init; }
    public StageManager Stages { get; private init; }
    public TextureSystem Textures { get; private init; }

    // NOTE: Leave graphics to be disposed by MainGame.
    public void Dispose()
    {
        Fonts.Dispose();
    }
}
