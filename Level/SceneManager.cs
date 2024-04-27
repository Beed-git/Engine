using Engine.Files;
using Engine.Resources;
using Microsoft.Extensions.Logging;

namespace Engine.Level;

public class SceneManager
{
    private readonly ILogger _logger;
    private readonly FileSystem _files;

    public SceneManager(ILoggerFactory loggerFactory, FileSystem files)
    {
        _logger = loggerFactory.CreateLogger<SceneManager>();
        _files = files;

        Next = null;
        Current = Scene.Empty;
    }

    public void ChangeScene(Resource name)
    {
        if (!_files.TryReadJsonAsset<Scene>(name, out var scene))
        {
            _logger.LogWarning("Scene of name '{}' does not exist.", name);
            return;
        }

        Next = scene;
    }

    public void ChangeScene(Scene scene)
    {
        Next = scene;
    }

    public Scene? Next { get; internal set; }
    public Scene Current { get; internal set; }
}
