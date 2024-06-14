namespace Engine.Core.Config;

public class ResourceSystemConfig
{
    public ResourceSystemConfig()
    {
        Resources = [];
    }

    public Dictionary<Type, object> Resources { get; init; }
}
