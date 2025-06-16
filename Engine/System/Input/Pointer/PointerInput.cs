using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.System.Input.Pointer;
using Minesweeper.System.Input.Pointer.Remote;

namespace Minesweeper.System.Input.Global;

public class PointerInput
{
    private static readonly Dictionary<PointerAction, PointerSnapshot> pointerStates = new();

    public static void Update()
    {
        pointerStates.Clear();

        foreach (PointerAction action in Enum.GetValues(typeof(PointerAction)))
        {
            var mouseState = GetMouseSnapshot(action);
            var touchState = GetTouchSnapshot(action);

            var chosen = mouseState.State switch
            {
                PointerState.Down or PointerState.Released => mouseState,
                _ => touchState.State != PointerState.None ? touchState : mouseState
            };

            pointerStates[action] = chosen;
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
                    Inside(new Rectangle((int)pos.X, (int)pos.Y, 1, 1)) ?
                        PointerState.Held : PointerState.None;

        return new PointerSnapshot(pos, action, state);
    }

    public static bool WasClicked(PointerAction action) =>
        pointerStates.TryGetValue(action, out var s) && s.State == PointerState.Down;

    public static bool WasReleased(PointerAction action) =>
        pointerStates.TryGetValue(action, out var s) && s.State == PointerState.Released;

    public static bool IsHeld(PointerAction action) =>
        pointerStates.TryGetValue(action, out var s) && s.State == PointerState.Held;

    public static Vector2 GetPosition(PointerAction action) =>
        pointerStates.TryGetValue(action, out var s) ? s.Position : Vector2.Zero;

    public static bool Inside(Rectangle bounds) =>
        pointerStates.Values.Any(s => bounds.Contains(s.Position));
    
    public static RemotePointerBatch GetCurrentInputBatch()
    {
        return new RemotePointerBatch
        {
            Events = Enum.GetValues(typeof(PointerAction))
                .Cast<PointerAction>()
                .Select(action => new RemotePointerEvent(
                    action,
                    GetPointerState(action),
                    GetPosition(action)))
                .Where(e => e.State != PointerState.None)
                .ToList()
        };
    }

    public static PointerState GetPointerState(PointerAction action)
    {
        if (WasClicked(action)) return PointerState.Down;
        if (WasReleased(action)) return PointerState.Released;
        if (IsHeld(action)) return PointerState.Held;
        return PointerState.None;
    }
    
    public static IInputSource GetInputSource(InputNode node)
    {
        return node switch
        {
            InputNode.Local => new LocalInputAdapter(),
            InputNode.Remote => new RemoteInputAdapter(),
            _ => throw new ArgumentOutOfRangeException(nameof(node))
        };
    }
}