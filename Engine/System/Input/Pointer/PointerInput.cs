using Minesweeper.System.Input.Mouse;
using Swipe.Android.System.Input.Touch;

namespace Minesweeper.System.Input.Global;

public class PointerInput
{
    public static void Update()
    {
        
    }

    public static bool WasClicked(PointerAction action)
    {
        return MouseManager.WasClicked(action) || TouchManager.WasClicked(action);
    }
}