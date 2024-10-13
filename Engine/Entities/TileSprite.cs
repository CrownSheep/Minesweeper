using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.DataHolders;
using Minesweeper.Graphics;

namespace Minesweeper.Entities;

public class TileSprite : Sprite
{
    private readonly int yOffset;
    private readonly int spriteIndex;

    public TileSprite(Texture2D spriteSheet, int spriteIndex, int yOffset = 0) : base(spriteSheet, spriteIndex * GridTile.TILE_WIDTH,
        47 + yOffset, GridTile.TILE_WIDTH, GridTile.TILE_HEIGHT)
    {
        this.spriteIndex = spriteIndex;
        this.yOffset = yOffset;
    }
}