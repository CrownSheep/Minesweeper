using System;

namespace Minesweeper;

public class IncompatibleAndroidService : IAndroidService
{
    public void Vibrate(long milliseconds, int amplitude = 1)
    {
        // Console.WriteLine($"[IncompatibleAndroidService] Vibrate({milliseconds}, amplitude: {amplitude}) called on non-Android device.");
    }

    public void ConsoleLog(string? prefix, string message)
    {
        // Console.WriteLine($"[IncompatibleAndroidService] ConsoleLog(prefix: {prefix ?? "null"}, message: {message}) called on non-Android device.");
    }
}