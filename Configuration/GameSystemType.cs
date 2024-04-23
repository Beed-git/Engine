namespace Engine.Configuration;

[Flags]
public enum GameSystemType
{
    None = 0,
    Update = 1,
    Render = 2,
    DebugUI = 4,
}
