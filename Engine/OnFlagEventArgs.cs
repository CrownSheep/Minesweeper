using System;

namespace Minesweeper;

public class OnFlagEventArgs(bool flagged) : EventArgs
{
    public bool Flagged { get; } = flagged;
}