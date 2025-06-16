using Microsoft.Xna.Framework;
using Minesweeper.System.Input.Global;
using Minesweeper.System.Input.Pointer;

public class LocalInputAdapter : IInputSource
{
    public bool WasClicked(PointerAction action) => PointerInput.WasClicked(action);
    public bool WasReleased(PointerAction action) => PointerInput.WasReleased(action);
    public bool IsHeld(PointerAction action) => PointerInput.IsHeld(action);
    public Vector2 GetPosition(PointerAction action) => PointerInput.GetPosition(action);
    public bool Inside(Rectangle bounds) => PointerInput.Inside(bounds);
}