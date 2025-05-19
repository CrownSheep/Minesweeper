using System;
using Minesweeper.System;

namespace Minesweeper;

public class OnClickEventArgs(MouseButton button, bool userClick = true) : EventArgs
{
    public MouseButton Button { get; } = button;

    public bool UserClick { get; } = userClick;
}