using Engine.Core.Utility;

namespace Engine.Core.Internal;

internal interface IEventHandler
{
    public string Name { get; }
    public Profiler Profiler { get; }
}
