using Engine.Configuration.Builders;

namespace Engine;

public static class GameEngine
{
    public static EngineSetupConfigBuilder Configure()
    {
        var builder = new EngineSetupConfigBuilder();
        return builder;
    }
}
