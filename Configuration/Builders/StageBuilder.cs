using System.Collections;

namespace Engine.Configuration.Builders;

public class StageBuilder
    : IEnumerable<KeyValuePair<string, Action<StageConfig, Dependencies>>>
{
    private readonly Dictionary<string, Action<StageConfig, Dependencies>> _configs;

    public StageBuilder(string initial, Action<StageConfig, Dependencies> initialConfig)
    { 
        InitialStage = initial;
        _configs = [];
        _configs.Add(initial, initialConfig);
    }

    internal string InitialStage { get; private init; }
    internal IEnumerable<KeyValuePair<string, Action<StageConfig, Dependencies>>> Stages => _configs;

    public void Add(string name, Action<StageConfig, Dependencies> config)
    {
        _configs.Add(name, config);
    }

    public IEnumerator<KeyValuePair<string, Action<StageConfig, Dependencies>>> GetEnumerator()
    {
        return _configs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
