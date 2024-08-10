using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper.NineSliceSystem;

public record FrameElement(
    int textureX,
    int textureY,
    int textureWidth,
    int textureHeight,
    int width = 0,
    int height = 0)
{
    public void Draw(SpriteBatch spriteBatch, Texture2D spriteSheet, int x, int y)
    {
        for (int i = 0; i < Math.Max(1, width / textureWidth); i++)
        {
            spriteBatch.Draw(spriteSheet, new Vector2(x + i * textureWidth, y),
                new Rectangle(textureX, textureY, textureWidth, textureHeight), Color.White);
        }

        if (width % textureWidth != 0)
            spriteBatch.Draw(spriteSheet,
                new Vector2(Math.Max(1, width / textureWidth) * textureWidth - Math.Max(1, width) % textureWidth, y),
                new Rectangle(textureX, textureY, textureWidth, textureHeight),
                Color.White);
        
        for (int i = 0; i < Math.Max(1, height / textureHeight); i++)
        {
            spriteBatch.Draw(spriteSheet, new Vector2(x, y + i * textureHeight),
                new Rectangle(textureX, textureY, textureWidth, textureHeight), Color.White);
        }

        if (height % textureHeight != 0)
            spriteBatch.Draw(spriteSheet,
                new Vector2(x, Math.Max(1, height / textureHeight) * textureHeight - Math.Max(1, height) % textureHeight),
                new Rectangle(textureX, textureY, textureWidth, textureHeight),
                Color.White);
    }
}