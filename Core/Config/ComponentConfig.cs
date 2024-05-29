namespace Engine.Core.Config;

public class ComponentConfig
{
    public ComponentConfig()
    {
        IsAutoRegistered = false;
    }

    public bool IsAutoRegistered { get; set; }
}
