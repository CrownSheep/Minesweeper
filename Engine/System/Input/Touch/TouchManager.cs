using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Swipe.Android.System.Input.Touch;

public static class TouchManager
{
    private static TouchCollection touch;
    private static readonly Stopwatch holdTimer = Stopwatch.StartNew();

    static TouchManager() {
        holdTimer.Reset();
    }

    public static void Update()
    {
        touch = TouchPanel.GetState();
    }

    public static int GetFingerCount()
    {
        return touch.Count;
    }

    public static Vector2? GetFingerPosition(int fingerIndex = 0)
    {
        if(!HasIndex(fingerIndex))
            return null;
        
        return touch[fingerIndex].Position;
    }

    public static float? GetFingerX(int fingerIndex = 0)
    {
        return GetFingerPosition(fingerIndex)?.X;
    }

    public static float? GetFingerY(int fingerIndex = 0)
    {
        return GetFingerPosition(fingerIndex)?.Y;
    }

    public static bool WasReleased(int fingerIndex = 0)
    {
        if(!HasIndex(fingerIndex))
            return false;
        
        return touch[fingerIndex].State == TouchLocationState.Released && holdTimer.IsRunning;
    }

    public static bool WasClicked(int fingerIndex = 0)
    {
        if(!HasIndex(fingerIndex))
            return false;
        
        return touch[fingerIndex].State == TouchLocationState.Pressed;
    }

    public static bool HeldOver(int fingerIndex = 0, int time = 200)
    {
        if (WasClicked(fingerIndex))
        {
            holdTimer.Restart();
        }

        if (holdTimer.ElapsedMilliseconds >= time)
        {
            holdTimer.Reset();
            return true;
        }

        return false;
    }
    
    private static bool HasIndex(TouchCollection panel, int fingerIndex = 0)
    {
        return panel.Count >= fingerIndex + 1;
    }

    public static bool HasIndex(int fingerIndex = 0)
    {
        return HasIndex(touch, fingerIndex);
    }

    public static bool Inside(Rectangle bounds, int fingerIndex = 0)
    {
        if(!HasIndex(fingerIndex))
            return false;
        
        Rectangle rect = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        return rect.Contains(
            new Vector2(GetFingerX(fingerIndex).Value, GetFingerY(fingerIndex).Value));
    }
}