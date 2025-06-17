using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using Minesweeper;
using Minesweeper.System.Input.Global;

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
        return !HasFinger() ? Vector2.Zero : touch[fingerIndex].Position;
    }

    public static float GetFingerX(int fingerIndex = 0)
    {
        return GetFingerPosition(fingerIndex).X;
    }

    public static float GetFingerY(int fingerIndex = 0)
    {
        return GetFingerPosition(fingerIndex).Y;
    }

    public static bool WasReleased(PointerAction action)
    {
        if(!HasFinger())
            return false;
        
        return action switch
        {
            PointerAction.Primary => touch.First().State == TouchLocationState.Released && !stillHeldOver,
            PointerAction.Secondary => WasReleased(PointerAction.Primary) && stillHeldOver,
            _ => false
        };
    }

    public static bool WasClicked(PointerAction action)
    {
        if(!HasFinger())
            return false;

        return action switch
        {
            PointerAction.Primary => touch.First().State == TouchLocationState.Pressed,
            PointerAction.Secondary => HeldOver(),
            _ => false
        };
    }

    private static bool HeldOver(int time = SECONDARY_CLICK_TIME)
    {
        if (!HasFinger())
        {
            stillHeldOver = false;
            return false;
        }

        if (WasClicked(PointerAction.Primary))
        {
            stillHeldOver = false;
            holdTimer.Restart();
        }

        if (holdTimer.ElapsedMilliseconds < time) return false;
        
        stillHeldOver = true;
        holdTimer.Reset();
        return true;
    }
    
    private static bool HasFinger(TouchCollection panel)
    {
        return panel.Count > 0;
    }

    public static bool HasFinger()
    {
        return HasFinger(touch);
    }

    public static bool Inside(Rectangle bounds)
    {
        if(!HasFinger())
            return false;
        
        Rectangle rect = new Rectangle(bounds.X, bounds.Y, bounds.Width, bounds.Height);
        
        return rect.Contains(
            new Vector2(GetFingerX(), GetFingerY()));
    }
}