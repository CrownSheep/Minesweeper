using System;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper.System;

public static class KeyboardInputManager
{
    private static KeyboardState KeyboardState;
    private static KeyboardState previousKeyboardState;

    public static bool WasJustPressed(Keys key)
    {
        KeyboardState = Keyboard.GetState();

        bool isKeyPressed = KeyboardState.IsKeyDown(key);
        bool wasKeyPressed = previousKeyboardState.IsKeyDown(key);

        previousKeyboardState = KeyboardState;
        

        return !wasKeyPressed && isKeyPressed;
    }
}