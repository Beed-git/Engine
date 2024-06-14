using Engine.Level;

namespace Engine.Events;

public readonly struct SceneAddEvent
{
    public readonly Scene Scene;

    public SceneAddEvent(Scene scene)
    {
        Scene = scene;
    }
}