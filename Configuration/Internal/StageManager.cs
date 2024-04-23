namespace Engine.Configuration.Internal;

public class StageManager
{
    private readonly Dependencies _dependencies;
    private readonly Dictionary<string, Action<StageConfig, Dependencies>> _stages;

    public StageManager(IEnumerable<KeyValuePair<string, Action<StageConfig, Dependencies>>> stages, Dependencies dependencies, string initialStageName)
    {
        _stages = stages.ToDictionary();
        _dependencies = dependencies;

        if (!_stages.TryGetValue(initialStageName, out var initial))
        {
            throw new InvalidOperationException($"Stage with name '{initialStageName}' doesn't exist.");
        }
        var stage = BuildStage(initialStageName, initial);
        Current = stage;
    }

    internal Stage Current { get; private set; }

    public string CurrentName => Current.Name;
    public IEnumerable<string> ValidStages => _stages.Keys;

    public void ChangeStage(string name)
    {
        var current = Current;
        if (!_stages.TryGetValue(name, out var configure)) 
        {
            throw new InvalidOperationException($"Stage with name '{name}' doesn't exist.");
        }

        Current = BuildStage(name, configure);
    }

    private Stage BuildStage(string name, Action<StageConfig, Dependencies> configure)
    {
        var config = new StageConfig(name);
        configure.Invoke(config, _dependencies);

        var stage = new Stage(name,
            _dependencies.LoggerFactory,
            config.UpdateSystems,
            config.RenderSystems,
            config.DebugUIs);

        return stage;
    } 
}
