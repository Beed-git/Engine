using Engine.Resources;

namespace Engine.Events;

public readonly struct SceneSetEvent
{
    public readonly ResourceName NextSceneName;

    public SceneSetEvent(ResourceName nextSceneName)
    {
        NextSceneName = nextSceneName;
    }
}