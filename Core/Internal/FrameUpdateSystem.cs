using Engine.Core.Utility;
using Engine.Level;
using Microsoft.Xna.Framework;

namespace Engine.Core.Internal;

public record FrameUpdateSystem(string Name, Action<Scene, GameTime> Action, Profiler Profile)
{
    public FrameUpdateSystem(string name, Action<Scene, GameTime> action)
        : this(name, action, new Profiler())
    {
    }
}