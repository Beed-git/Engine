using Engine.Files;
using Engine.Resources;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Engine.Level;

public class SceneManager
{
    private readonly ILogger _logger;
    private readonly FileSystem _files;

    private readonly Scene _empty;
    private readonly Dictionary<ResourceName, Scene> _cache;

    public SceneManager(ILoggerFactory loggerFactory, FileSystem files)
    {
        _logger = loggerFactory.CreateLogger<SceneManager>();
        _files = files;
        _cache = [];

        _empty = Create("empty");

        Next = null;
        Current = _empty;
    }

    public Scene? Next { get; internal set; }
    public Scene Current { get; internal set; }

    public Scene Create(ResourceName name)
    {
        if (_cache.TryGetValue(name, out var scene))
        {
            _logger.LogWarning("Attempted to create scene with name '{}' but scene already exists. Returning cached scene.", name);
            return scene;
        }

        if (_files.Exists(name))
        {
            _logger.LogWarning("A scene with the name '{}' was dynamically created but an asset with the same name exists on the disk. Did you mean to load this scene instead?", name);
        }

        scene = new Scene(name);
        _cache.Add(name, scene);    
        return scene;
    }

    // TODO: Replace with event.
    public void ChangeScene(Scene scene)
    {
        _cache.TryAdd(scene.Name, scene);
        Next = scene;
    }

    private bool TryGetScene(ResourceName name, [MaybeNullWhen(false)] out Scene scene) 
    {
        if (_cache.TryGetValue(name, out scene!))
        {
            _logger.LogInformation("Found scene '{}' in cache.", name);
            return true;
        }

        if (_files.TryReadJsonAsset(name, out scene!))
        {
            _logger.LogInformation("Loaded scene '{}' from file.", name);
            return true;
        }

        scene = null;
        _logger.LogWarning("Failed find a scene with the name '{}'.", name);
        return false;
    }
}
