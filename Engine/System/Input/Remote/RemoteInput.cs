using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Minesweeper.System.Input.Global;

namespace Minesweeper.System.Input.Pointer.Remote;

public static class RemoteInput
{
    private static readonly PointerSnapshot[] pointerSnapshots = new PointerSnapshot[Enum.GetValues<PointerAction>().Length];
    
    public static void UpdateSnapshots(PointerSnapshot[] snapshots)
    {
        if (snapshots.Length != pointerSnapshots.Length)
            return;

        Array.Copy(snapshots, pointerSnapshots, snapshots.Length);
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
        pointerSnapshots.Any(s => bounds.Contains(ScreenSpaceUtils.ToZoomedPosition(s.Position)));
    
    public static PointerState GetPointerState(PointerAction action)
    {
        if (WasClicked(action)) return PointerState.Down;
        if (WasReleased(action)) return PointerState.Released;
        if (IsHeld(action)) return PointerState.Held;
        return PointerState.None;
    }

    private static PointerSnapshot GetSnapshot(PointerAction action)
        => pointerSnapshots[(int)action];
}
