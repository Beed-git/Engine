using Microsoft.Xna.Framework.Input;

namespace Engine.Util;

internal static class MouseStateExtensions
{
    public static bool IsMouseButtonDown(this MouseState state, MouseButton button)
    {
        return button switch
        {
            MouseButton.Mouse1 => state.LeftButton == ButtonState.Pressed,
            MouseButton.Mouse2 => state.RightButton == ButtonState.Pressed,
            MouseButton.Mouse3 => state.MiddleButton == ButtonState.Pressed,
            MouseButton.Mouse4 => state.XButton1 == ButtonState.Pressed,
            MouseButton.Mouse5 => state.XButton2 == ButtonState.Pressed,
            _ => throw new Exception($"Unexpected mouse button '{button}'")
        };
    }

    public static bool IsMouseButtonUp(this MouseState state, MouseButton button)
    {
        return button switch
        {
            MouseButton.Mouse1 => state.LeftButton == ButtonState.Released,
            MouseButton.Mouse2 => state.RightButton == ButtonState.Released,
            MouseButton.Mouse3 => state.MiddleButton == ButtonState.Released,
            MouseButton.Mouse4 => state.XButton1 == ButtonState.Released,
            MouseButton.Mouse5 => state.XButton2 == ButtonState.Released,
            _ => throw new Exception($"Unexpected mouse button '{button}'")
        };
    }
}
