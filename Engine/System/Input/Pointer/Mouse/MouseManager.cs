using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.System.Input.Global;
using static Microsoft.Xna.Framework.Input.Mouse;

namespace Minesweeper.System.Input.Mouse;

public static class MouseManager
{
    private const int SCROLL_UNIT = 120;
    
    private static MouseState mouseState, oldMouseState;

    public static void Update()
    {
        oldMouseState = mouseState;
        mouseState = GetState();
    }

    private static ButtonState GetButtonState(PointerAction action, bool old = false)
    {
        return action switch
        {
            PointerAction.Primary => old ? oldMouseState.LeftButton : mouseState.LeftButton,
            PointerAction.Secondary => old ? oldMouseState.RightButton : mouseState.RightButton,
            PointerAction.Tertiary => old ? oldMouseState.MiddleButton : mouseState.MiddleButton,
            _ => old ? oldMouseState.LeftButton : mouseState.LeftButton
        };
    }
    
    public static float GetMouseX()
    {
        return mouseState.X;
    }
    
    public static float GetMouseY()
    {
        return mouseState.Y;
    }
    
    public static Vector2 GetMousePosition()
    {
        return new Vector2(mouseState.X, mouseState.Y);
    }

    public static int GetScrollWheelValue()
    {
        return mouseState.ScrollWheelValue / SCROLL_UNIT;
    }
    
    public static int GetOldScrollWheelValue()
    {
        return oldMouseState.ScrollWheelValue / SCROLL_UNIT;
    }
    
    public static int GetScrolledValue()
    {
        return GetScrollWheelValue() - GetOldScrollWheelValue();
    }
    
    public static ScrollDirection GetCurrentScrollDirection()
    {
        return Math.Sign(GetScrolledValue()) switch
        {
            1 => ScrollDirection.Up,
            -1 => ScrollDirection.Down,
            _ => ScrollDirection.None
        };
    }
    
    public static bool JustScrolledInDirection(ScrollDirection direction = ScrollDirection.None)
    {
        return direction switch
        {
            ScrollDirection.Up => GetScrollWheelValue() > GetOldScrollWheelValue(),
            ScrollDirection.Down => GetScrollWheelValue() < GetOldScrollWheelValue(),
            _ => GetScrollWheelValue() != GetOldScrollWheelValue()
        };
    }

    public static bool WasReleased(PointerAction button)
    {
        ButtonState buttonState = GetButtonState(button);
        ButtonState oldButtonState = GetButtonState(button, true);

        return buttonState == ButtonState.Released && oldButtonState == ButtonState.Pressed;
    }

    public static bool WasClicked(PointerAction button)
    {
        ButtonState buttonState = GetButtonState(button);
        ButtonState oldButtonState = GetButtonState(button, true);
        
        return buttonState == ButtonState.Pressed && oldButtonState == ButtonState.Released;
    }
    
    public static bool IsCurrently(ButtonState buttonState, PointerAction button)
    {        
        return GetButtonState(button) == buttonState;
    }
    
    public static bool Inside(Rectangle bounds)
    {
        return bounds.Contains(new Vector2(mouseState.X, mouseState.Y));
    }
}