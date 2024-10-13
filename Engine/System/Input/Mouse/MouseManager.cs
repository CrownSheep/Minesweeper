using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper.System.Input.Mouse;

public static class MouseManager
{
    private static MouseState mouseState, oldMouseState;

    public static void Update()
    {
        oldMouseState = mouseState;
        mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();
    }

    private static ButtonState GetButtonState(MouseButtons button, bool old = false)
    {
        return button switch
        {
            MouseButtons.Left => old ? oldMouseState.LeftButton : mouseState.LeftButton,
            MouseButtons.Right => old ? oldMouseState.RightButton : mouseState.RightButton,
            MouseButtons.Middle => old ? oldMouseState.MiddleButton : mouseState.MiddleButton,
            _ => old ? oldMouseState.LeftButton : mouseState.LeftButton
        };
    }

    public static int GetScrollWheelValue()
    {
        return mouseState.ScrollWheelValue / 120;
    }
    
    public static int GetOldScrollWheelValue()
    {
        return oldMouseState.ScrollWheelValue / 120;
    }
    
    public static int GetScrolledValue()
    {
        return GetScrollWheelValue() - GetOldScrollWheelValue();
    }
    
    public static ScrollDirection? GetCurrentScrollDirection()
    {
        return Math.Sign(GetScrolledValue()) switch
        {
            1 => ScrollDirection.Up,
            -1 => ScrollDirection.Down,
            0 => null
        };
    }
    
    public static bool JustScrolledInDirection(ScrollDirection direction = ScrollDirection.Both)
    {
        return direction switch
        {
            ScrollDirection.Both => GetScrollWheelValue() != GetOldScrollWheelValue(),
            ScrollDirection.Up => GetScrollWheelValue() > GetOldScrollWheelValue(),
            ScrollDirection.Down => GetScrollWheelValue() < GetOldScrollWheelValue(),
            _ => GetScrollWheelValue() != GetOldScrollWheelValue()
        };
    }

    public static bool WasReleased(MouseButtons button)
    {
        ButtonState buttonState = GetButtonState(button);
        ButtonState oldButtonState = GetButtonState(button, true);

        return buttonState == ButtonState.Released && oldButtonState == ButtonState.Pressed;
    }

    public static bool WasClicked(MouseButtons button)
    {
        ButtonState buttonState = GetButtonState(button);
        ButtonState oldButtonState = GetButtonState(button, true);
        
        return buttonState == ButtonState.Pressed && oldButtonState == ButtonState.Released;
    }
    
    public static bool IsCurrently(ButtonState buttonState, MouseButtons button)
    {        
        return GetButtonState(button) == buttonState;
    }
    
    public static bool Hover(Rectangle bounds)
    {
        return bounds.Contains(new Vector2(mouseState.X, mouseState.Y));
    }
}