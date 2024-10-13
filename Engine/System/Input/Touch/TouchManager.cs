using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Minesweeper;

namespace Swipe.Android.System.Input.Touch;

public class TouchManager
{
    private static TouchCollection touchState;
    private static Stopwatch holdTimer = Stopwatch.StartNew();
    private static Stopwatch clickTimer = Stopwatch.StartNew();
    private static bool putFlagged;

    public static void Update()
    {
        touchState = TouchPanel.GetState();
        if (!IsFingerPressed())
            putFlagged = false;
    }

    public static void ResetHoldTime(bool restart = false)
    {
        if (restart)
        {
            holdTimer.Restart();
        }
        else
        {
            holdTimer.Reset();
        }

        putFlagged = true;
    }

    public static int GetFingerCount()
    {
        return touchState.Count;
    }

    public static bool HasFinger()
    {
        return GetFingerCount() > 0;
    }
    
    public static Vector2? GetLastFingerHoldPosition(int fingerIndex = 0)
    {
        return GetLastFingerHoldPosition(fingerIndex).HasValue ? HeldOver() ? GetFingerPosition() : null : null;
    }

    public static Vector2? GetFingerPosition(int fingerIndex = 0)
    {
        return touchState[fingerIndex].Position;
    }

    public static float? GetFingerX(int fingerIndex = 0)
    {
        return GetFingerPosition(fingerIndex).Value.X;
    }

    public static float? GetMouseY(int fingerIndex = 0)
    {
        return GetFingerPosition(fingerIndex).Value.Y;
    }

    public static bool WasReleased(int fingerIndex = 0)
    {
        if (!HasFinger())
            return false;

        return touchState[fingerIndex].State == TouchLocationState.Released;
    }

    public static bool WasClicked(int fingerIndex = 0)
    {
        if (!HasFinger())
            return false;

        return touchState[fingerIndex].State == TouchLocationState.Pressed;
    }

    public static bool PutFlagged()
    {
        return putFlagged;
    }

    public static bool HeldOver(int fingerIndex = 0, int time = 200)
    {
        if (WasClicked(fingerIndex))
        {
            holdTimer.Restart();
        }

        return holdTimer.ElapsedMilliseconds >= time;
    }

    private static bool IsFingerPressed(TouchCollection panel, int fingerIndex = 0)
    {
        if (!HasFinger())
            return false;

        return panel.Count >= fingerIndex + 1;
    }

    public static bool IsFingerPressed(int fingerIndex = 0)
    {
        if (!HasFinger())
            return false;

        return touchState.Count >= fingerIndex + 1;
    }

    public static bool Inside(Rectangle bounds, int fingerIndex = 0)
    {
        if (!HasFinger())
            return false;

        Rectangle rect = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        return rect.Contains(
            new Vector2(GetFingerPosition(fingerIndex).Value.X, GetFingerPosition(fingerIndex).Value.Y));
    }
}