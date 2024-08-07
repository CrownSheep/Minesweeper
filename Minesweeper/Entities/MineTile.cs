using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.Graphics;

namespace Minesweeper.Entities;

public class MineTile : Clickable
{
    public int DrawOrder => 10;

    public const int TILE_WIDTH = 18;
    public const int TILE_HEIGHT = 18;

    private Sprite hiddenTileSprite;
    
    private Sprite emptyTileSprite;
    
    private Sprite bombTileSprite;
    private Sprite clickedBombTileSprite;

    public Sprite Sprite { get; private set; }
    
    public int Index { get; private set; }
    public bool Hidden { get; set; }

    private Texture2D spriteSheet;
    
    public event EventHandler RevealEvent;

    public int xIndex;
    public int yIndex;

    public Vector2 indexs;

    public MineTile(Texture2D spriteSheet, Vector2 position, int width, int height, int xIndex, int yIndex) : base(position,
        width, height)
    {
        this.spriteSheet = spriteSheet;
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        indexs = new Vector2(xIndex, yIndex);
        
        Index = 0;
        Hidden = true;

        hiddenTileSprite = new Sprite(spriteSheet, 0, 47, TILE_WIDTH, TILE_HEIGHT);
        emptyTileSprite = new Sprite(spriteSheet, TILE_WIDTH, 47, TILE_WIDTH, TILE_HEIGHT);
        bombTileSprite = new Sprite(spriteSheet, 90, 47, TILE_WIDTH, TILE_HEIGHT);
        clickedBombTileSprite = new Sprite(spriteSheet, 90 + TILE_WIDTH, 47, TILE_WIDTH, TILE_HEIGHT);

        Sprite = hiddenTileSprite;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }


    private Sprite getSpriteByIndex(int index)
    {
        switch (index)
        {
            case <= -2:
                return clickedBombTileSprite;
            case -1:
                return bombTileSprite;
            case 0:
                return emptyTileSprite;
            case > 0:
                return new Sprite(spriteSheet, 0 + MathHelper.Clamp(index - 1, 0, 7) * TILE_WIDTH, 65, TILE_WIDTH,
                    TILE_HEIGHT);
        }
    }

    public void SetIndex(int index)
    {
        Index = MathHelper.Clamp(index, -2, 8);
    }
    
    public void setSpriteByIndex(int index)
    {
        setSprite(getSpriteByIndex(index));
    }

    protected override void OnLeftMouseClick()
    {
        Reveal();
    }

    public void Reveal()
    {
        if (Hidden)
        {
            OnRevealEvent();
            Hidden = false;
            setSpriteByIndex(Index);
        }
    }

    public void setSprite(Sprite sprite)
    {
        Sprite = sprite;
    }
    
    protected virtual void OnRevealEvent()
    {
        EventHandler handler = RevealEvent;
        handler?.Invoke(this, EventArgs.Empty);
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        if (Hidden)
        {
            hiddenTileSprite.Draw(spriteBatch, Position);
        }
        else
        {
            getSpriteByIndex(Index).Draw(spriteBatch, Position);
        }
    }
}