using System;

namespace Minesweeper.DataHolders;

public static class Egg
{
    private static int? _seed;

    static Egg()
    {
        _seed = Environment.TickCount;
        Main.client.SendMessage(MessageType.BoardSeed, _seed);
    }

    public static void SetSeed(int seed)
    {
       _seed = seed;
    }

    public static int GetSeed()
    {
        int? seed = _seed;
        _seed = null;
        return seed.GetValueOrDefault();
    }
    
    public static bool HasSeed()
    {
        return _seed.HasValue;
    }
}