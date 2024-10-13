namespace Minesweeper.DataHolders;

public record GameConfig(int width, int height, int bombCount, bool showBombsAtStart)
{
    public static readonly GameConfig MOBILE = new(9, 16, 21, false);
    public static readonly GameConfig BEGINNER = new(9, 9, 10, false);
    public static readonly GameConfig INTERMEDIATE = new(16, 16, 40, false);
    public static readonly GameConfig EXPERT = new(30, 16, 99, false);
    
    public int width { get; } = width;
    public int height { get; } = height;
    public int bombCount { get; } = bombCount;
    public bool showBombsAtStart { get; } = showBombsAtStart;
}