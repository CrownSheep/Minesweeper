using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Minesweeper.System.Input.Global;

namespace Minesweeper.System.Input.Pointer.Remote;

public static class RemoteInput
{
    private static PointerSnapshot[] pointerSnapshots = new PointerSnapshot[Enum.GetValues<PointerAction>().Length];
    
    public static void FromServer(PointerSnapshot[] remotePointerSnapshots)
    {
        pointerSnapshots = remotePointerSnapshots;
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
}
