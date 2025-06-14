using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Minesweeper.System.Input.Global;

namespace Swipe.Android.System.Input.Touch;

public static class TouchManager
{
    private const int SECONDARY_CLICK_TIME = 200;
    
    private static TouchCollection touch;
    private static readonly Stopwatch holdTimer = Stopwatch.StartNew();

    private static bool stillHeldOver = false;
        
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

    public static Vector2 GetFingerPosition(int fingerIndex = 0)
    {
        return !HasIndex(fingerIndex) ? Vector2.Zero : touch[fingerIndex].Position;
    }

    public static float GetFingerX(int fingerIndex = 0)
    {
        return GetFingerPosition(fingerIndex).X;
    }

    public static float GetFingerY(int fingerIndex = 0)
    {
        return GetFingerPosition(fingerIndex).Y;
    }

    public static bool WasReleased(PointerAction action, int fingerIndex = 0)
    {
        if(!HasIndex(fingerIndex))
            return false;
        
        return action switch
        {
            PointerAction.Primary => touch[fingerIndex].State == TouchLocationState.Released,
            PointerAction.Secondary => WasReleased(PointerAction.Primary) && stillHeldOver,
            _ => false
        };
    }

    public static bool WasClicked(PointerAction action, int fingerIndex = 0)
    {
        if(!HasIndex(fingerIndex))
            return false;

        return action switch
        {
            PointerAction.Primary => touch[fingerIndex].State == TouchLocationState.Pressed,
            PointerAction.Secondary => HeldOver(fingerIndex),
            _ => false
        };
    }

    private static bool HeldOver(int fingerIndex = 0, int time = SECONDARY_CLICK_TIME)
    {
        if (!HasIndex(fingerIndex))
        {
            stillHeldOver = false;
            return false;
        }

        if (WasClicked(PointerAction.Primary, fingerIndex))
        {
            stillHeldOver = false;
            holdTimer.Restart();
        }

        if (holdTimer.ElapsedMilliseconds < time) return false;
        
        stillHeldOver = true;
        holdTimer.Reset();
        return true;
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
            new Vector2(GetFingerX(fingerIndex), GetFingerY(fingerIndex)));
    }
}