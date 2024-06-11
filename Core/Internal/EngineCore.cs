using Engine.Data;
using Engine.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Engine.Level;
using Engine.Core.Config;
using Engine.Core.Config.Factories;
using Engine.Resources;
using Engine.Resources.Loaders;

namespace Engine.Core.Internal;

internal class EngineCore
{
    private readonly ILogger _logger;
    private readonly EngineConfig _config;

    public EngineCore(EngineConfig config, GraphicsDeviceManager graphicsDeviceManager)
    {
        _logger = config.LoggerFactory.CreateLogger<EngineCore>();
        _config = config;

        Dependencies = BuildCoreServices(graphicsDeviceManager);
        StageFactory = BuildStages(Dependencies);
    }

    public StaticServices Dependencies { get; private init; }
    public StageFactory StageFactory { get; private init; }

    private StaticServices BuildCoreServices(GraphicsDeviceManager graphicsDeviceManager)
    {
        _logger.LogInformation("Starting engine.");

        _logger.LogInformation("Registering components.");
        var components = ComponentRegistryFactory.Create(_config.ComponentConfig, _config.LoggerFactory);

        _logger.LogInformation("Finished registering components.");

        _logger.LogInformation("Building dependencies.");

        var serializer = SerializerFactory.Create(_config.SerializationConfig, components);
        var files = FileSystemFactory.Create(_config.FileSystemConfig, serializer);
        var database = new Database(_config.LoggerFactory, files);
        var screen = new Screen(graphicsDeviceManager);
        var fontSystem = FontSystemFactory.Create(_config.FontConfig, files);
        var textures = new TextureSystem(_config.LoggerFactory, graphicsDeviceManager.GraphicsDevice, files);
        var stages = new StageManager(_config.LoggerFactory, files);

        // TODO: Move resource initialization somewhere else.
        var resources = new ResourceSystem(files, serializer);
        resources.AddResourceLoader(new TileSetResourceLoader(serializer, textures));

        var statics = new StaticServices
        {
            Database = database,
            Files = files,
            Fonts = fontSystem,
            LoggerFactory = _config.LoggerFactory,
            Resources = resources,
            Screen = screen,
            Stages = stages,
            Textures = textures,
        };

        _logger.LogInformation("Finished building dependencies.");

        statics.Stages.Next = _config.StageCollectionConfig.InitialStageName;

        return statics;
    }

    private StageFactory BuildStages(StaticServices dependencies)
    {
        var repo = new StageFactory(_config.LoggerFactory, dependencies, _config.StageCollectionConfig.StageBuilders);
        return repo;
    }
}

