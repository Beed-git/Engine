using Engine.Data;
using Engine.ECS;
using Engine.Files;
using Engine.Level;
using Engine.Rendering;
using Engine.Resources;
using FontStashSharp;
using Microsoft.Extensions.Logging;

namespace Engine.Core.Config;

public class StaticServices
    : IDisposable
{
    public required ILoggerFactory LoggerFactory { get; init; }
    public required Database Database { get; init; }
    public required FileSystem Files { get; init; }
    public required FontSystem Fonts { get; init; }
    public required ResourceSystem Resources { get; init; }
    public required Screen Screen { get; init; }
    public required StageManager Stages { get; init; }
    public required TextureSystem Textures { get; init; }

    // NOTE: Leave graphics to be disposed by MainGame.
    public void Dispose()
    {
        Fonts.Dispose();
    }
}

public class Services
{
    public Services(
        StaticServices statics,
        SceneManager scenes,
        EventSystem events)
    {
        SceneManager = scenes;
        Events = events;

        LoggerFactory = statics.LoggerFactory;
        Database = statics.Database;
        FileSystem = statics.Files;
        Fonts = statics.Fonts;
        Screen = statics.Screen;
        Stages = statics.Stages;
        Resources = statics.Resources;
        Textures = statics.Textures;
    }

    public SceneManager SceneManager { get; init; }
    public EventSystem Events { get; init; }

    // TEMP: Copied from StaticServices.
    public ILoggerFactory LoggerFactory { get; init; }
    public Database Database { get; init; }
    public FileSystem FileSystem { get; init; }
    public FontSystem Fonts { get; init; }
    public ResourceSystem Resources { get; init; }
    public Screen Screen { get; init; }
    public StageManager Stages { get; init; }
    public TextureSystem Textures { get; init; }
}
