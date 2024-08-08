using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Graphics;

namespace Minesweeper.Entities;

public class MineTile : Clickable
{
    public int DrawOrder => 10;

    public const int TILE_WIDTH = 18;
    public const int TILE_HEIGHT = 18;

    private Sprite hiddenTileSprite;
    private Sprite flaggedTileSprite;
    private Sprite wrongfulFlaggedTileSprite;
    
    private Sprite emptyTileSprite;
    
    private Sprite bombTileSprite;
    private Sprite clickedBombTileSprite;

    public Sprite Sprite { get; private set; }
    
    public int Index { get; private set; }
    public bool Hidden { get; set; }
    public bool Flagged { get; set; }
    public bool ShouldDisplayWrongfulFlagged { get; set; }

    private Texture2D spriteSheet;
    
    public event EventHandler ClickEvent;
    public event EventHandler FlagEvent;

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
        Flagged = false;

        hiddenTileSprite = new Sprite(spriteSheet, 0, 47, TILE_WIDTH, TILE_HEIGHT);
        flaggedTileSprite = new Sprite(spriteSheet, 36, 47, TILE_WIDTH, TILE_HEIGHT);
        wrongfulFlaggedTileSprite = new Sprite(spriteSheet, 126, 47, TILE_WIDTH, TILE_HEIGHT);
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
        if (Hidden)
        {
            if (Flagged)
            {
                if (!isBomb() && ShouldDisplayWrongfulFlagged)
                {
                    return wrongfulFlaggedTileSprite;
                }
                return flaggedTileSprite;
            }

            return hiddenTileSprite;
        }
        
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
        Reveal(Mouse.GetState().LeftButton);
    }
    
    protected override void OnRightMouseClick()
    {
        Flag();
    }
    
    public void Flag()
    {
        Flagged = !Flagged;
        OnFlagEvent();
    }

    public void Reveal(ButtonState buttonState)
    {
        if (!Flagged)
        {
            if (Hidden)
            {
                OnClickEvent(buttonState);
                Hidden = false;
                setSpriteByIndex(Index);
            }
        }
    }

    public void setSprite(Sprite sprite)
    {
        Sprite = sprite;
    }
    
    protected virtual void OnClickEvent(ButtonState buttonState)
    {
        EventHandler handler = ClickEvent;
        handler?.Invoke(this, new OnClickEventArgs(buttonState));
    }
    
    protected virtual void OnFlagEvent()
    {
        EventHandler handler = FlagEvent;
        handler?.Invoke(this, EventArgs.Empty);
    }

    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        getSpriteByIndex(Index).Draw(spriteBatch, Position);
    }
    
    public bool isBomb()
    {
        return Index == -1;
    }
    
    public bool isEmpty()           
    {
        return Index == 0;
    }

}