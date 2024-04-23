namespace Engine.Configuration;

public class ComponentConfig
{
    public ComponentConfig()
    {
        IsAutoRegistered = false;
    }

    internal bool IsAutoRegistered { get; private set; }

    public void AutoRegister()
    {
        IsAutoRegistered = true;
    }
}
