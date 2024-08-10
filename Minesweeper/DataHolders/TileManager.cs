using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Entities;
using Minesweeper.Graphics;

namespace Minesweeper.DataHolders;

public class TileManager : Clickable
{
    public int DrawOrder => 10;

    public const int TILE_WIDTH = 18;
    public const int TILE_HEIGHT = 18;
    
    private const int BOMB_INDEX = -1;
    private const int EMPTY_INDEX = 0;

    private Sprite HiddenSprite => new Sprite(spriteSheet, 0, 47, TILE_WIDTH, TILE_HEIGHT);
    private Sprite FlagSprite => new Sprite(spriteSheet, 36, 47, TILE_WIDTH, TILE_HEIGHT);
    private Sprite BadFlagSprite => new Sprite(spriteSheet, 126, 47, TILE_WIDTH, TILE_HEIGHT);
    private Sprite EmptySprite => new Sprite(spriteSheet, TILE_WIDTH, 47, TILE_WIDTH, TILE_HEIGHT);
    private Sprite BombSprite => new Sprite(spriteSheet, 90, 47, TILE_WIDTH, TILE_HEIGHT);
    private Sprite ClickedBombSprite => new Sprite(spriteSheet, 90 + TILE_WIDTH, 47, TILE_WIDTH, TILE_HEIGHT);
    
    public int Index { get; private set; }
    public bool Hidden { get; set; }
    public bool Flagged { get; set; }
    public bool IsBadFlagged { get; set; }
    public bool ClickedBomb { get; set; }

    private readonly Texture2D spriteSheet;
    public event EventHandler ClickEvent;
    public event EventHandler RevealEvent;
    public event EventHandler FlagEvent;

    public int xIndex;
    public int yIndex;

    public Vector2 indexs;

    public TileManager(Game1 game,Texture2D spriteSheet, Vector2 position, int width, int height, int xIndex, int yIndex) : base(game, position,
        width, height)
    {
        this.spriteSheet = spriteSheet;
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        indexs = new Vector2(xIndex, yIndex);
        
        Index = 0;
        Hidden = true;
        Flagged = false;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    private Sprite GetSpriteByIndex(int index)
    {
        if (Hidden)
        {
            if (Flagged)
            {
                if (!IsBomb() && IsBadFlagged)
                {
                    return BadFlagSprite;
                }
                return FlagSprite;
            }

            return HiddenSprite;
        }

        if (ClickedBomb)
            return ClickedBombSprite;
        
        switch (index)
        {
            case <= BOMB_INDEX:
                return BombSprite;
            case 0:
                return EmptySprite;
        }
        return new Sprite(spriteSheet, 0 + MathHelper.Clamp(index - 1, 0, 7) * TILE_WIDTH, 65, TILE_WIDTH,
            TILE_HEIGHT);
    }

    public void SetIndex(int index)
    {
        Index = MathHelper.Clamp(index, BOMB_INDEX, 8);
    }

    public bool IsHidden()
    {
        return Hidden;
    }

    protected override void OnLeftMouseClick()
    {
        OnClickEvent(Mouse.GetState().LeftButton);
    }
    
    protected override void OnRightMouseClick()
    {
        OnClickEvent(Mouse.GetState().RightButton);
    }
    
    public void Flag()
    {
        Flagged = !Flagged;
        OnFlagEvent(Flagged);
    }

    public void Reveal()
    {
        if (!Flagged)
        {
            if (Hidden)
            {
                OnRevealEvent();
            }
        }
    }
    
    protected virtual void OnClickEvent(ButtonState buttonState)
    {
        EventHandler handler = ClickEvent;
        handler?.Invoke(this, new OnClickEventArgs(buttonState));
    }
    
    protected virtual void OnRevealEvent()
    {
        EventHandler handler = RevealEvent;
        handler?.Invoke(this, EventArgs.Empty);
    }
    
    protected virtual void OnFlagEvent(bool flagged)
    {
        EventHandler handler = FlagEvent;
        handler?.Invoke(this, new OnFlagEventArgs(flagged));
    }

    public bool IsBomb()
    {
        return Index == BOMB_INDEX;
    }
    
    public bool IsEmpty()
    {
        return Index == EMPTY_INDEX;
    }
    public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        GetSpriteByIndex(Index).Draw(spriteBatch, Position);
    }
    
}