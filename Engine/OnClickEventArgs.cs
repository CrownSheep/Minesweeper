using System;
using Minesweeper.System;
using Minesweeper.System.Input.Global;

namespace Minesweeper;

public class OnClickEventArgs(PointerAction action, bool userClick = true) : EventArgs
{
    public PointerAction Action { get; } = action;

    public bool UserClick { get; } = userClick;
}