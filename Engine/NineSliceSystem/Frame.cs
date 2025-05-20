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
        elements =
        [
            topLeft, 
            middleTop, 
            topRight, 
            middleLeft, 
            middleRight, 
            bottomLeft, 
            bottomMiddle, 
            bottomRight
        ];
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D spriteSheet)
    {
        for (int i = 0; i < elements.Length; i++)
        {
            int posX = X;
            int posY = Y;

            switch (i)
            {
                // Calculate X position
                case 2:
                case 4:
                // Right aligned elements
                case 7:
                    posX = Width - elements[i].textureWidth;
                    break;
                case 1:
                // Center elements horizontally
                case 6:
                    posX += elements[0].textureWidth;
                    break;
            }

            switch (i)
            {
                // Calculate Y position
                // Middle row
                case >= 3 and <= 4:
                    posY += elements[0].textureHeight;
                    break;
                // Bottom row
                case >= 5:
                    posY += Height - elements[i].textureHeight;
                    break;
            }

            elements[i].Draw(spriteBatch, spriteSheet, posX, posY);
        }
    }

    public FrameElement getElementByIndex(int index)
    {
        return elements[index];
    }
}