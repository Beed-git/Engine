using Engine.Data;
using Engine.Files;
using Engine.Rendering;
using Engine.Serialization.Converters;
using Engine.Serialization;
using FontStashSharp;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;
using Engine.Configuration.Builders;
using Microsoft.Xna.Framework;
using Engine.ECS;
using Engine.Level;

namespace Engine.Configuration.Internal;

internal class EngineCore
{
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;
    private readonly EngineSetupConfig _config;
    private readonly StageBuilder _stages;

    private const string FontExtension = "ttf";

    internal EngineCore(ILoggerFactory loggerFactory, EngineSetupConfig config, StageBuilder stages)
    {
        _logger = loggerFactory.CreateLogger<EngineCore>();
        _loggerFactory = loggerFactory;
        _config = config;
        _stages = stages;
    }

    public Dependencies BuildDependencies(GraphicsDeviceManager graphicsDeviceManager)
    {
        _logger.LogInformation("Starting engine.");

        var registry = RegisterComponents();
        var dependencies = CreateDependencies(graphicsDeviceManager, registry);

        dependencies.Stages.Next = _stages.InitialStage;

        return dependencies;
    }

    public StageRepository BuildStages(Dependencies dependencies)
    {
        var repo = new StageRepository(_loggerFactory, dependencies, _stages);
        return repo;
    }

    private ComponentRegistry RegisterComponents()
    {
        _logger.LogInformation("Registering components.");

        var registry = new ComponentRegistry(_loggerFactory);
        var config = new ComponentConfig();
        if (_config.ComponentConfig is null)
        {
            // TODO: Ensure components are registered in builders.
            throw new Exception("Components must be configured.");
        }
        _config.ComponentConfig.Invoke(config);

        if (config.IsAutoRegistered)
        {
            AutoRegisterComponents(registry);
        }

        _logger.LogInformation("Finished registering components.");
        return registry;
    }

    private Dependencies CreateDependencies(GraphicsDeviceManager graphicsDeviceManager, ComponentRegistry registry)
    {
        _logger.LogInformation("Building dependencies.");
        var serializer = BuildSerializer(registry);
        var fileSystem = BuildFileSystem(serializer);
        var database = new Database(_loggerFactory, fileSystem);
        var screen = new Screen(graphicsDeviceManager);
        var fontSystem = BuildFontSystem(fileSystem);
        var textures = new TextureSystem(_loggerFactory, graphicsDeviceManager.GraphicsDevice, fileSystem);
        var stages = new StageManager(_loggerFactory, fileSystem);

        var dependencies = new Dependencies(
            _loggerFactory,
            database,
            fileSystem,
            fontSystem,
            screen,
            stages,
            textures);

        _logger.LogInformation("Finished building dependencies.");
        return dependencies;
    }

    // Helpers

    private Serializer BuildSerializer(ComponentRegistry registry)
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            IncludeFields = true,
            IgnoreReadOnlyFields = true,
            IgnoreReadOnlyProperties = true,
            PropertyNameCaseInsensitive = true,
            AllowTrailingCommas = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        var config = new SerializationConfig();
        _config.SerializationConfiguration?.Invoke(config);

        foreach (var converter in config.Converters)
        {
            jsonSerializerOptions.Converters.Add(converter);
        }

        if (config.UseDefaultConverters)
        {
            jsonSerializerOptions.Converters.Add(new TemplateConverter(registry));
        }

        var serializer = new Serializer(jsonSerializerOptions);
        return serializer;
    }

    private FileSystem BuildFileSystem(Serializer serializer)
    {
        var config = new FileSystemConfig();
        _config.FileSystemConfig?.Invoke(config);

        var root = config.Root
            ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? throw new Exception("Root asset directory was not set and reflection fallback failed.");

        var fileSystemSettings = new FileSystemSettings(root);

        var fileSystem = new FileSystem(fileSystemSettings, serializer);
        return fileSystem;
    }

    private FontSystem BuildFontSystem(FileSystem files)
    {
        var config = new FontConfig();
        _config.FontConfiguration?.Invoke(config);

        var fontPath = config.OverrideFallbackFont
            ?? throw new NotImplementedException("Default font has not been added.");

        var resource = $"{FileSystemSettings.FontsFolder}{fontPath}";
        if (!files.TryReadBinary(resource, FontExtension, out var font))
        {
            throw new Exception($"Failed to find font resource at path '{resource}'");
        }

        var fontSystem = new FontSystem();
        fontSystem.AddFont(font);

        return fontSystem;
    }

    private static void AutoRegisterComponents(ComponentRegistry registry)
    {
        const string ComponentEnding = "Component";

        var types =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.IsDefined(typeof(ComponentAttribute)))
            .Concat(
                Assembly.GetEntryAssembly()?
                    .GetTypes()
                    .Where(t => t.IsDefined(typeof(ComponentAttribute))) ?? []);

        foreach (var type in types)
        {
            var name = type.Name;
            if (name.EndsWith(ComponentEnding, StringComparison.OrdinalIgnoreCase))
            {
                name = name[..^ComponentEnding.Length];
            }

            if (name.Length == 0)
            {
                throw new Exception($"Generated name is invalid for struct '{type}' (name is '{type}')");
            }

            name = char.ToLower(name[0]) + name[1..];
            registry.Register(type, name);
        }
    }
}

