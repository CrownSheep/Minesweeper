using Microsoft.Xna.Framework;

namespace Minesweeper.Extensions;

public static class Vector2Ext
{
    public static bool IsWithinAdjacentZone(this Vector2 position, Vector2 otherPosition, int offset = 1)
    {
        for (int x = -1 * offset; x <= 1 * offset; x++)
        {
            for (int y = -1 * offset; y <= 1 * offset; y++)
            {
                if (position == new Vector2(otherPosition.X + x, otherPosition.Y + y))
                {
                    return true;
                }
            }
        }

        return false;
    }
}