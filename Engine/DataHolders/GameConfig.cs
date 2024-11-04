namespace Minesweeper.DataHolders;

public record GameConfig(int width, int height, int bombCount, bool showBombsAtStart)
{
    public static readonly GameConfig Beginner = new(9, 9, 10, false);
    public static readonly GameConfig Intermediate = new(16, 16, 40, false);
    public static readonly GameConfig Expert = new(30, 16, 99, false);
}