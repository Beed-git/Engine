using Engine.Core.Utility;
using Engine.Level;
using Microsoft.Xna.Framework;

namespace Engine.Core.Internal;

internal record FrameUpdateSystem(string Name, Action<Scene, GameTime> Action, Profiler Profile)
{
    internal FrameUpdateSystem(string name, Action<Scene, GameTime> action)
        : this(name, action, new Profiler())
    {
    }
}