using Microsoft.Xna.Framework.Graphics;
using Minesweeper.Graphics;

namespace Minesweeper.DataHolders;

public record Sprites(Texture2D texture, int x, int y, int width, int height)
{
    public static Sprite Smile => new(Globals.TransparentSpriteSheet, 4, 27, 17, 17);
}