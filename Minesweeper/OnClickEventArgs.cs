using System;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper;

public class OnClickEventArgs : EventArgs
{
    private readonly ButtonState button;

    public OnClickEventArgs(ButtonState button)
    {
        this.button = button;
    }
    
    public ButtonState Button
    {
        get { return button; }
    }
}