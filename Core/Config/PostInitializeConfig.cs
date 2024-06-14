namespace Engine.Core.Config;

public class PostInitializeConfig
{ 
    public required Func<StaticServices, ResourceSystemConfig> CreateResourceConfig { get; init; }
}