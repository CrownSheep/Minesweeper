using System;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper;

public class OnFlagEventArgs : EventArgs
{
    private readonly bool flagged;

    public OnFlagEventArgs(bool flagged)
    {
        this.flagged = flagged;
    }
    
    public bool Flagged
    {
        get { return flagged; }
    }
}