using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.DataHolders;

namespace Minesweeper.Entities;

public record GameStateSprite(int spriteIndex)
{
    public void Draw(SpriteBatch spriteBatch, Texture2D spriteSheet, Vector2 position)
    {
        spriteBatch.Draw(spriteSheet, position, new Rectangle(spriteIndex * GameStateManager.SPRITE_WIDTH, 23, GameStateManager.SPRITE_WIDTH, GameStateManager.SPRITE_HEIGHT), Color.White);
    }
}