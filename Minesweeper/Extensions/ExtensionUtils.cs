using System;

namespace Minesweeper.Extensions;

public static class ExtensionUtils
{
    public static void ThrowNullError<T>(ref T value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));
    }
}