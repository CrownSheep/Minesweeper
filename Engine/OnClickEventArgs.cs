using System;
using Microsoft.Xna.Framework.Input;
using Minesweeper.System;

namespace Minesweeper;

public class OnClickEventArgs : EventArgs
{
    private readonly MouseButtons button;
    private readonly bool userClick;

    public OnClickEventArgs(MouseButtons button, bool userClick = true)
    {
        this.button = button;
        this.userClick = userClick;
    }
    
    public MouseButtons Button => button;
    public bool UserClick => userClick;
}