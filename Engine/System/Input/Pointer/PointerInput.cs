using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.System.Input.Pointer;
using Minesweeper.System.Input.Pointer.Remote;

namespace Minesweeper.System.Input.Global;

public static class PointerInput
{
    private static readonly PointerSnapshot[] pointerSnapshots = new PointerSnapshot[Enum.GetValues<PointerAction>().Length];

    public static void Update()
    {
        foreach (PointerAction action in Enum.GetValues(typeof(PointerAction)))
        {
            var mouse = GetMouseSnapshot(action);
            var touch = GetTouchSnapshot(action);

            var chosen = mouse.State switch
            {
                PointerState.Down or PointerState.Released => mouse,
                _ => touch.State != PointerState.None ? touch : mouse
            };

            pointerSnapshots[(int)action] = chosen;
        }
    }

    private static PointerSnapshot GetMouseSnapshot(PointerAction action)
    {
        bool isDown = MouseManager.IsCurrently(ButtonState.Pressed, action);
        bool wasDown = MouseManager.IsCurrently(ButtonState.Pressed, action, true);

        var state = isDown switch
        {
            true when !wasDown => PointerState.Down,
            true => PointerState.Held,
            false when wasDown => PointerState.Released,
            _ => PointerState.None
        };

        return new PointerSnapshot(MouseManager.GetMousePosition(), action, state);
    }

    private static PointerSnapshot GetTouchSnapshot(PointerAction action)
    {
        if (!TouchManager.HasFinger())
            return new PointerSnapshot(Vector2.Zero, action, PointerState.None);

        var pos = TouchManager.GetFingerPosition();
        var state = TouchManager.WasClicked(action) ? PointerState.Down :
            TouchManager.WasReleased(action) ? PointerState.Released :
            Inside(new Rectangle((int)pos.X, (int)pos.Y, 1, 1)) ? PointerState.Held : PointerState.None;

        return new PointerSnapshot(pos, action, state);
    }

    public static bool WasClicked(PointerAction action) =>
        GetSnapshot(action).State == PointerState.Down;

    public static bool WasReleased(PointerAction action) =>
        GetSnapshot(action).State == PointerState.Released;

    public static bool IsHeld(PointerAction action) =>
        GetSnapshot(action).State == PointerState.Held;

    public static Vector2 GetPosition(PointerAction action) =>
        GetSnapshot(action).Position;

    public static bool Inside(Rectangle bounds) =>
        pointerSnapshots.Any(s => bounds.Contains(s.Position));
    
    private static PointerSnapshot GetSnapshot(PointerAction action)
        => pointerSnapshots[(int)action];

    public static PointerState GetPointerState(PointerAction action)
    {
        if (WasClicked(action)) return PointerState.Down;
        if (WasReleased(action)) return PointerState.Released;
        if (IsHeld(action)) return PointerState.Held;
        return PointerState.None;
    }
    
    public static PointerSnapshot[] GetPointerSnapshots() => pointerSnapshots;

    public static IInputSource GetInputSource(InputSide side)
    {
        return side switch
        {
            InputSide.Local => new LocalInputAdapter(),
            InputSide.Remote => new RemoteInputAdapter(),
            _ => throw new ArgumentOutOfRangeException(nameof(side))
        };
    }
}