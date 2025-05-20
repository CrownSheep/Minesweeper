using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper.Extensions;

public static class SpriteBatchExt
{
    private static Texture2D texture;
    public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        ExtensionUtils.ThrowNullError(ref spriteBatch);

        if (texture == null)
        {
            texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            texture.SetData([color]);
        }

        spriteBatch.Draw(texture, rectangle, color);
    }
    
    public static void DrawBorder(this SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth) {
        ExtensionUtils.ThrowNullError(ref spriteBatch);
        
        if (texture == null)
        {
            texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            texture.SetData([color]);
        }

        spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
        spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
        spriteBatch.Draw(texture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
        spriteBatch.Draw(texture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
    }
}