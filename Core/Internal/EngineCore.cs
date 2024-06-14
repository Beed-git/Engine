using Engine.Data;
using Engine.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Engine.Level;
using Engine.Core.Config;
using Engine.Core.Config.Factories;
using Engine.Resources;
using System.Reflection;

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
        var fontSystem = FontSystemFactory.Create(_config.FontConfig, files);
        var resources = new ResourceSystem(files, serializer);
        var screen = new Screen(graphicsDeviceManager);
        var stages = new StageManager(_config.LoggerFactory, files);
        var textures = new TextureSystem(graphicsDeviceManager.GraphicsDevice);

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

        _logger.LogInformation("Initializing resources.");

        var resourceConfig = _config.PostInitializeConfig.CreateResourceConfig.Invoke(statics);
        foreach (var (type, config) in resourceConfig.Resources)
        {;
            var method = typeof(ResourceSystem).GetMethod(nameof(ResourceSystem.AddConfig), BindingFlags.Instance | BindingFlags.NonPublic);
            var methodRef = method!.MakeGenericMethod(type);
            methodRef.Invoke(resources, [config]);
        }

        _logger.LogInformation("Finished initializing resources.");


        statics.Stages.Next = _config.StageCollectionConfig.InitialStageName;

        return statics;
    }

    private StageFactory BuildStages(StaticServices dependencies)
    {
        var repo = new StageFactory(_config.LoggerFactory, dependencies, _config.StageCollectionConfig.StageBuilders);
        return repo;
    }
}

