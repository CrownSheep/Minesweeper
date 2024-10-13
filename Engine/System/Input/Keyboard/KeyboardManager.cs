using System;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper.System.Input.Keyboard;

public static class KeyboardManager
{
    private static KeyboardState keyboardState, oldKeyboardState;
    private static bool[] savedKeyStates;
    
    public static void Update()
    {
        oldKeyboardState = keyboardState;
        keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();
    }
    
    public static bool WasKeyDown(Keys key)
    {
        bool isKeyPressed = IsKeyDown(key);
        bool wasKeyPressed = oldKeyboardState.IsKeyDown(key);

        return !wasKeyPressed && isKeyPressed;
    }
    
    public static bool IsKeyDown(Keys key)
    {
        return keyboardState.IsKeyDown(key);
    }
    
    public static bool AreKeysDown(MultiKey key)
    {
        return AreKeysDown(key.Keys);
    }

    public static bool WereKeysDown(MultiKey key)
    {
        return WereKeysDown(key.Keys);
    }
    
    public static bool AreKeysDown(params Keys[] keys)
    {
        return keys.All(IsKeyDown);
    }

    public static bool WereKeysDown(params Keys[] keys)
    {
        if (savedKeyStates == null || savedKeyStates.Length != keys.Length)
        {
            savedKeyStates = new bool[keys.Length];
        }

        for (int i = 0; i < keys.Length; i++)
        {
            if (IsKeyDown(keys[i]) && !oldKeyboardState.IsKeyDown(keys[i]))
            {
                savedKeyStates[i] = true;
            }
        }

        if (savedKeyStates.All(state => state))
        {
            Array.Clear(savedKeyStates, 0, savedKeyStates.Length);
            return true;
        }

        return false;
    }
}