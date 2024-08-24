using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper.System.Input.Keyboard;

public static class KeyboardInputManager
{
    private static KeyboardState KeyboardState, previousKeyboardState;
    
    public static void Update()
    {
        previousKeyboardState = KeyboardState;
        KeyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
    }
    
    public static bool WasKeyDown(Keys key)
    {
        bool isKeyPressed = IsKeyDown(key);
        bool wasKeyPressed = previousKeyboardState.IsKeyDown(key);

        return !wasKeyPressed && isKeyPressed;
    }
    
    public static bool IsKeyDown(Keys key)
    {
        return KeyboardState.IsKeyDown(key);
    }
    
    public static bool AreKeysDown(MultiKey key)
    {
        return key.Keys.All(IsKeyDown);
    }

    public static bool WereKeysDown(MultiKey key)
    {
        return key.Keys.All(key => !previousKeyboardState.IsKeyDown(key) && IsKeyDown(key));
    }
    
    public static bool AreKeysDown(params Keys[] keys)
    {
        return keys.All(IsKeyDown);
    }

    public static bool WereKeysDown(params Keys[] keys)
    {
        return keys.All(key => !previousKeyboardState.IsKeyDown(key) && IsKeyDown(key));
    }
    
}