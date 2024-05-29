namespace Engine.Core.Config;

public class StageCollectionConfig
{
    public StageCollectionConfig()
    {
        StageBuilders = [];
        InitialStageName = string.Empty;
    }

    public string InitialStageName { get; set; }
    public Dictionary<string, Action<StageConfig, Dependencies>> StageBuilders { get; private init; }
}
