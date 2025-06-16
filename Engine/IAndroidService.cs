namespace Minesweeper;

public interface IAndroidService
{
    void Vibrate(long milliseconds, int amplitude = 1);
    void ConsoleLog(string? prefix, string message);
}