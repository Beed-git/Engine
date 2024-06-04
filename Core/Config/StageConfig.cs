using Engine.Core.Internal;

namespace Engine.Core.Config;

public class StageConfig
{
    public StageConfig()
    {
        DebugUIs = [];
        EventRegisterSystems = [];
        UpdateSystems = [];
        RenderSystems = [];
    }

    public List<FrameUpdateSystem> DebugUIs { get; private init; }
    public List<FrameUpdateSystem> UpdateSystems { get; private init; }
    public List<FrameUpdateSystem> RenderSystems { get; private init; }
    public List<KeyValuePair<string, Action<EventRegistry>>> EventRegisterSystems { get; private init; }
}