using System;
using Microsoft.Xna.Framework;
using Minesweeper.System.Input.Global;

namespace Minesweeper.System.Input.Pointer.Remote;

[Serializable]
public struct RemotePointerEvent(PointerAction action, PointerState state, Vector2 position)
{
    public PointerAction Action = action;
    public PointerState State = state;
    public Vector2 Position = position;
}