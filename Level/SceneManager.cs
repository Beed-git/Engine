using Engine.Configuration;
using Engine.Files;
using Microsoft.Extensions.Logging;

namespace Engine.Level;

public class SceneManager
{
    private readonly ILogger _logger;

    public SceneManager(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<SceneManager>();
    }

    public Scene? Current { get; private set; }
    public Scene? Next { get; private set; }

    public void ChangeScene(Scene scene)
    {
        Next = scene;
    }

    internal void Update()
    {
        if (Next is not null)
        {
            _logger.LogInformation("{}", Current is not null
                ? $"Changing scene from {Current.Name} to {Next.Name}"
                : $"Changing scene to {Next.Name}");
        }
    }
}
