using System;
using Microsoft.Xna.Framework.Input;
using Minesweeper.System;

namespace Minesweeper;

public class OnClickEventArgs : EventArgs
{
    private readonly MouseButtons button;

    public OnClickEventArgs(MouseButtons button)
    {
        this.button = button;
    }
    
    public MouseButtons Button
    {
        get { return button; }
    }
}