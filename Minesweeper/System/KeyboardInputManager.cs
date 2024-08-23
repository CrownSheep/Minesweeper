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

        bool isKeyPressed = KeyboardState[key] == KeyState.Down;
        bool wasKeyPressed = previousKeyboardState[key] == KeyState.Down;

        previousKeyboardState = KeyboardState;
        

        return !wasKeyPressed && isKeyPressed;
    }
}