using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.DataHolders;

namespace Minesweeper.Entities;

public record TileSprite(int spriteIndex)
{
    private readonly int yOffset;
    public TileSprite(int spriteIndex, int yOffset) : this(spriteIndex)
    {
        this.yOffset = yOffset;
    }
    public void Draw(SpriteBatch spriteBatch, Texture2D spriteSheet ,Vector2 position)
    {
        spriteBatch.Draw(spriteSheet, position, new Rectangle(spriteIndex * GridManager.TILE_WIDTH, 47 + yOffset, GridManager.TILE_WIDTH, GridManager.TILE_HEIGHT), Color.White);
    }
}