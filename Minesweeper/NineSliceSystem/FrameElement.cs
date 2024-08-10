using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.Extensions;

namespace Minesweeper.NineSliceSystem;

public record FrameElement
{
    public readonly int textureX;
    public readonly int textureY;
    public readonly int textureWidth;
    public readonly int textureHeight;
    public readonly int width;
    public readonly int height;

    public FrameElement(int textureX,
        int textureY,
        int textureWidth,
        int textureHeight,
        int? width = null,
        int? height = null)
    {
        this.textureX = textureX;
        this.textureY = textureY;
        this.textureWidth = textureWidth;
        this.textureHeight = textureHeight;
        this.width = width ?? textureWidth;
        this.height = height ?? textureHeight;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D spriteSheet, int x, int y)
    {
        for (int i = 0; i < width / textureWidth; i++)
        {
            spriteBatch.Draw(spriteSheet, new Vector2(x + i * textureWidth, y),
                new Rectangle(textureX, textureY, textureWidth, textureHeight), Color.White);
        }

        if (width % textureWidth != 0)
        {
            spriteBatch.Draw(spriteSheet,
                new Vector2(x + width / textureWidth * textureWidth, y),
                new Rectangle(textureX, textureY, width % textureWidth, textureHeight),
                Color.White);
            spriteBatch.DrawBorder(
                new Rectangle(x + width / textureWidth * textureWidth, y, width % textureWidth, textureHeight),
                Color.Red, 1);
        }

        for (int i = 0; i < height / textureHeight; i++)
        {
            spriteBatch.Draw(spriteSheet, new Vector2(x, y + i * textureHeight),
                new Rectangle(textureX, textureY, textureWidth, textureHeight), Color.White);
        }

        if (height % textureHeight != 0)
        {
            spriteBatch.Draw(spriteSheet,
                new Vector2(x, y + height / textureHeight * textureHeight),
                new Rectangle(textureX, textureY, textureWidth, height % textureHeight),
                Color.White);
            spriteBatch.DrawBorder(
                new Rectangle(x, y + height / textureHeight * textureHeight, textureWidth, height % textureHeight),
                Color.Red, 1);
        }
    }
}