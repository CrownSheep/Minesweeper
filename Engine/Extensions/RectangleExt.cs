using Microsoft.Xna.Framework;

namespace Minesweeper.Extensions;

public static class RectangleExt
{
    public static Vector2 GetCenter(this Rectangle rectangle)
    {
        ExtensionUtils.ThrowNullError(ref rectangle);
        
        return new Vector2(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
    }
}