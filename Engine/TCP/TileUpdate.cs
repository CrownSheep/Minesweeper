namespace Minesweeper;

public class TileUpdate
{
    public enum TileState
    {
        Revealed,
        Flagged
    }
    
    public int X { get; set; }
    public int Y { get; set; }
    public TileState State { get; set; }
    public bool User { get; set; }
}