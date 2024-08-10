using Microsoft.Xna.Framework.Graphics;

namespace Minesweeper.NineSliceSystem;

public class Frame
{
    private readonly FrameElement[] elements;
    public int X { get; }
    public int Y { get; }
    public int Width { get; }
    public int Height { get; }

    public Frame(int x, int y, int width, int height,
        FrameElement topLeft, 
        FrameElement middleTop, 
        FrameElement topRight, 
        FrameElement middleLeft, 
        FrameElement middleRight, 
        FrameElement bottomLeft, 
        FrameElement bottomMiddle, 
        FrameElement bottomRight)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        elements = new FrameElement[]
        {
            topLeft, 
            middleTop, 
            topRight, 
            middleLeft, 
            middleRight, 
            bottomLeft, 
            bottomMiddle, 
            bottomRight
        };
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D spriteSheet)
    {
        elements[0].Draw(spriteBatch, spriteSheet, X, Y);
        elements[1].Draw(spriteBatch, spriteSheet, X + elements[0].textureWidth, Y);
        elements[2].Draw(spriteBatch, spriteSheet, Width - elements[2].textureWidth, Y);
        elements[3].Draw(spriteBatch, spriteSheet, X, Y + elements[0].textureHeight);
        elements[4].Draw(spriteBatch, spriteSheet, Width - elements[4].textureWidth, Y + elements[2].textureHeight);
        elements[5].Draw(spriteBatch, spriteSheet, X, Y + Height - elements[5].textureHeight);
        elements[6].Draw(spriteBatch, spriteSheet, X + elements[5].textureWidth, Y + Height - elements[5].textureHeight);
        elements[7].Draw(spriteBatch, spriteSheet, Width - elements[7].textureWidth, Y + Height - elements[7].textureHeight);
    }

    public FrameElement getElementByIndex(int index)
    {
        return elements[index];
    }
}
