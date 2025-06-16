using Microsoft.Xna.Framework;
using Minesweeper.System.Input.Global;

namespace Minesweeper.System.Input.Pointer.Remote;

public class RemoteInputAdapter : IInputSource
{
    public bool WasClicked(PointerAction action) => RemoteInput.WasClicked(action);
    public bool WasReleased(PointerAction action) => RemoteInput.WasReleased(action);
    public bool IsHeld(PointerAction action) => RemoteInput.IsHeld(action);
    public Vector2 GetPosition(PointerAction action) => RemoteInput.GetPosition(action);
    public bool Inside(Rectangle bounds) => RemoteInput.Inside(bounds);
}