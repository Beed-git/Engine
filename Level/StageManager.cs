using Engine.Configuration.Internal;
using Engine.Files;
using Microsoft.Extensions.Logging;

namespace Engine.Level;

public class StageManager
{
    internal StageManager(ILoggerFactory loggerFactory, FileSystem files)
    {
        CurrentStage = Stage.CreateEmpty(loggerFactory, files);
    }

    public void ChangeStage(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Next = name;
    }

    public string? Next { get; internal set; }
    public string Current => CurrentStage.Name;

    internal Stage CurrentStage { get; set; }
}
