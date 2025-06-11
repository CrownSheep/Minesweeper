using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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

    private static ButtonState GetButtonState(MouseButton button, bool old = false)
    {
        return button switch
        {
            MouseButton.Left => old ? oldMouseState.LeftButton : mouseState.LeftButton,
            MouseButton.Right => old ? oldMouseState.RightButton : mouseState.RightButton,
            MouseButton.Middle => old ? oldMouseState.MiddleButton : mouseState.MiddleButton,
            _ => old ? oldMouseState.LeftButton : mouseState.LeftButton
        };
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

    public static bool WasReleased(MouseButton button)
    {
        ButtonState buttonState = GetButtonState(button);
        ButtonState oldButtonState = GetButtonState(button, true);

        return buttonState == ButtonState.Released && oldButtonState == ButtonState.Pressed;
    }

    public static bool WasClicked(MouseButton button)
    {
        ButtonState buttonState = GetButtonState(button);
        ButtonState oldButtonState = GetButtonState(button, true);
        
        return buttonState == ButtonState.Pressed && oldButtonState == ButtonState.Released;
    }
    
    public static bool IsCurrently(ButtonState buttonState, MouseButton button)
    {        
        return GetButtonState(button) == buttonState;
    }
    
    public static bool Hover(Rectangle bounds)
    {
        return bounds.Contains(new Vector2(mouseState.X, mouseState.Y));
    }
}