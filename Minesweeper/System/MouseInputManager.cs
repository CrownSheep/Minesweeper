using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper.System;

public static class MouseInputManager
{
    private static bool LeftActualClicked;
    private static bool RightActualClicked;
    private static bool MiddleActualClicked;

    private static MouseState mouseState, oldMouseState;

    public static void Update()
    {
        oldMouseState = mouseState;
        mouseState = Mouse.GetState();
    }

    public static bool IsLeftClicked(bool checkHold)
    {        
        if (checkHold)
        {
            if (mouseState.LeftButton != ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                LeftActualClicked = true;
            }

            if (LeftActualClicked && mouseState.LeftButton == ButtonState.Released)
            {
                LeftActualClicked = false;
                return true;
            }
        }
        else
        {
            return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != ButtonState.Pressed;
        }

        return false;
    }
    
    public static bool IsLeftHold()
    {        
        return mouseState.LeftButton == ButtonState.Pressed;
    }
    
    public static bool IsRightHold()
    {        
        return mouseState.RightButton == ButtonState.Pressed;
    }
    
    public static bool IsRightClicked(bool checkHold)
    {
        if (checkHold)
        {
            if (mouseState.RightButton != ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Pressed)
            {
                RightActualClicked = true;
            }

            if (RightActualClicked && mouseState.RightButton == ButtonState.Released)
            {
                RightActualClicked = false;
                return true;
            }
        }
        else
        {
            return mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != ButtonState.Pressed;
        }

        return false;
    }
    
    public static bool Hover(Rectangle r)
    {
        return r.Contains(new Vector2(mouseState.X, mouseState.Y));
    }
}