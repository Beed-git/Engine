using Engine.Data;
using Engine.Rendering;
using Engine.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Engine.Level;
using Engine.Core.Config;
using Engine.Core.Config.Factories;

namespace Engine.Core.Internal;

internal class EngineCore
{
    private readonly ILogger _logger;
    private readonly EngineConfig _config;

    public EngineCore(EngineConfig config)
    {
        _logger = config.LoggerFactory.CreateLogger<EngineCore>();
        _config = config;
    }

    public Dependencies BuildDependencies(GraphicsDeviceManager graphicsDeviceManager)
    {
        _logger.LogInformation("Starting engine.");

        var components = RegisterComponents();
        var dependencies = CreateDependencies(graphicsDeviceManager, components);

        dependencies.Stages.Next = _config.StageCollectionConfig.InitialStageName;

        return dependencies;
    }

    internal StageRepository BuildStages(Dependencies dependencies)
    {
        var repo = new StageRepository(_config.LoggerFactory, dependencies, _config.StageCollectionConfig.StageBuilders);
        return repo;
    }

    private ComponentRegistry RegisterComponents()
    {
        _logger.LogInformation("Registering components.");

        var registry = ComponentRegistryFactory.Create(_config.ComponentConfig, _config.LoggerFactory);

        _logger.LogInformation("Finished registering components.");
        return registry;
    }

    private Dependencies CreateDependencies(GraphicsDeviceManager graphicsDeviceManager, ComponentRegistry components)
    {
        _logger.LogInformation("Building dependencies.");
        var serializer = SerializerFactory.Create(_config.SerializationConfig, components);
        var fileSystem = FileSystemFactory.Create(_config.FileSystemConfig, serializer);
        var database = new Database(_config.LoggerFactory, fileSystem);
        var screen = new Screen(graphicsDeviceManager);
        var fontSystem = FontSystemFactory.Create(_config.FontConfig, fileSystem);
        var textures = new TextureSystem(_config.LoggerFactory, graphicsDeviceManager.GraphicsDevice, fileSystem);
        var stages = new StageManager(_config.LoggerFactory, fileSystem);

        var dependencies = new Dependencies(
            _config.LoggerFactory,
            database,
            fileSystem,
            fontSystem,
            screen,
            stages,
            textures);

        _logger.LogInformation("Finished building dependencies.");
        return dependencies;
    }
}

