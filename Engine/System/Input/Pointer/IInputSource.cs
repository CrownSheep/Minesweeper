using Microsoft.Xna.Framework;
using Minesweeper.System.Input.Global;

namespace Minesweeper.System.Input.Pointer;

public interface IInputSource
{
    bool WasClicked(PointerAction action);
    bool WasReleased(PointerAction action);
    bool IsHeld(PointerAction action);
    Vector2 GetPosition(PointerAction action);
    bool Inside(Rectangle bounds);
}
