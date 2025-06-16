using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Minesweeper.System.Input.Global;

namespace Minesweeper.System.Input.Pointer.Remote;

public static class RemoteInput
{
    private static Dictionary<PointerAction, RemotePointerEvent> events = new();

    public static void Apply(RemotePointerBatch batch)
    {
        events.Clear();
        foreach (RemotePointerEvent e in batch.Events)
            events[e.Action] = e;
    }

    public static bool WasClicked(PointerAction action) =>
        events.TryGetValue(action, out RemotePointerEvent e) && e.State == PointerState.Down;

    public static bool WasReleased(PointerAction action) =>
        events.TryGetValue(action, out RemotePointerEvent e) && e.State == PointerState.Released;

    public static bool IsHeld(PointerAction action) =>
        events.TryGetValue(action, out RemotePointerEvent e) && e.State == PointerState.Held;

    public static Vector2 GetPosition(PointerAction action) =>
        events.TryGetValue(action, out RemotePointerEvent e) ? e.Position : Vector2.Zero;

    public static bool Inside(Rectangle bounds) =>
        events.Values.Any(e => bounds.Contains(e.Position));
}
