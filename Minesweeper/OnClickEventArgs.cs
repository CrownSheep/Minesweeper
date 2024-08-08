using System;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper;

public class OnClickEventArgs : EventArgs
{
    public readonly ButtonState button;

    public OnClickEventArgs(ButtonState button)
    {
        this.button = button;
    }
}