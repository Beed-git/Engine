using Microsoft.Xna.Framework.Input;

namespace Engine.Util;

public static class DirectInput
{
    private static KeyboardState s_keyboardCurrent;
    private static KeyboardState s_keyboardLast;

    private static MouseState s_mouseCurrent;
    private static MouseState s_mouseLast;

    internal static void Update()
    {
        s_mouseLast = s_mouseCurrent;
        s_keyboardLast = s_keyboardCurrent;

        s_mouseCurrent = Mouse.GetState();
        s_keyboardCurrent = Keyboard.GetState();
    }

    public static bool IsKeyDown(Keys key)
    {
        return s_keyboardCurrent.IsKeyDown(key);
    }

    public static bool IsKeyUp(Keys key)
    {
        return s_keyboardCurrent.IsKeyUp(key);
    }

    public static bool IsKeyJustPressed(Keys key)
    {
        return s_keyboardCurrent.IsKeyDown(key) && s_keyboardLast.IsKeyUp(key);
    }

    public static bool IsMouseButtonUp(MouseButton button)
    {
        return s_mouseCurrent.IsMouseButtonUp(button);
    }

    public static bool IsMouseButtonDown(MouseButton button)
    {
        return s_mouseCurrent.IsMouseButtonDown(button);
    }

    public static bool IsMouseButtonJustPressed(MouseButton button)
    {
        return s_mouseCurrent.IsMouseButtonDown(button) && s_mouseLast.IsMouseButtonUp(button);
    }
}