using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Entities;
using Minesweeper.System;
using Minesweeper.System.Input.Mouse;

namespace Minesweeper.DataHolders;

public class GridTile : Clickable
{
    public const int TILE_WIDTH = 18;
    public const int TILE_HEIGHT = 18;

    public const int BOMB_INDEX = -1;
    public const int EMPTY_INDEX = 0;
    
    private static readonly TileSprite hiddenSprite = new(Globals.MainSpriteSheet, 0);
    private static readonly TileSprite emptySprite = new(Globals.MainSpriteSheet, 1);
    private static readonly TileSprite flagSprite = new(Globals.MainSpriteSheet, 2);
    private static readonly TileSprite bombSprite = new(Globals.MainSpriteSheet, 5);
    private static readonly TileSprite clickedBombSprite = new(Globals.MainSpriteSheet, 6);
    private static readonly TileSprite badFlagSprite = new(Globals.MainSpriteSheet, 7);
    public int Index { get; set; }
    public bool Hidden { get; set; }
    public bool ShowHeld { get; set; }
    public bool Flagged { get; set; }
    public bool IsBadFlagged { get; set; }
    public bool ClickedBomb { get; set; }
    public event EventHandler ClickEvent;
    public event EventHandler RevealEvent;
    public event EventHandler FlagEvent;

    public int xIndex;
    public int yIndex;

    public Vector2 gridPosition;

    public GridTile(Game1 game, Vector2 position, int width, int height, int xIndex, int yIndex) : base(game, position,
        width, height, MouseButtons.Right)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        gridPosition = new Vector2(xIndex, yIndex);
        
        Index = 0;
        Hidden = true;
        Flagged = false;
    }

    public void SetIndex(int index)
    {
        Index = MathHelper.Clamp(index, BOMB_INDEX, 8);
    }

    public bool IsHidden()
    {
        return Hidden;
    }

    public override void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();
        if (game.IsActive && game.GameState != GameState.Lose && game.GameState != GameState.Win)
        {
            if (Bounds.Contains(mouseState.Position))
            {
                if (Hidden && !Flagged)
                {
                    game.GameState = mouseState.LeftButton == ButtonState.Pressed ? GameState.Held : GameState.Playing;
                    ShowHeld = mouseState.LeftButton == ButtonState.Pressed;
                }
            }
            else
            {
                if(MouseManager.WasReleased(MouseButtons.Left))
                    game.GameState = GameState.Playing;
                
                ShowHeld = false;
            }
        }
        
        base.Update(gameTime);
    }


    protected override void OnLeftMouseClick()
    {
        if (game.GameState != GameState.Lose && game.GameState != GameState.Win)
        {
            OnClickEvent(MouseButtons.Left);
        }
    }
    
    protected override void OnRightMouseClick()
    {
        if(game.GameState != GameState.Lose && game.GameState != GameState.Win)
            OnClickEvent(MouseButtons.Right);
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
    
    
    
    protected virtual void OnClickEvent(MouseButtons button)
    {
        EventHandler handler = ClickEvent;
        handler?.Invoke(this, new OnClickEventArgs(button));
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
    
    public static TileSprite GetSpriteByTile(GridTile tile)
    {
        if (tile.Hidden)
        {
            if (tile.ShowHeld)
                return emptySprite;

            if (tile.Flagged)
            {
                if (!tile.IsBomb() && tile.IsBadFlagged)
                {
                    return badFlagSprite;
                }

                return flagSprite;
            }

            return hiddenSprite;
        }

        if (tile.ClickedBomb)
            return clickedBombSprite;

        switch (tile.Index)
        {
            case <= BOMB_INDEX:
                return bombSprite;
            case 0:
                return emptySprite;
        }

        return new TileSprite(Globals.MainSpriteSheet, 0 + MathHelper.Clamp(tile.Index - 1, 0, 7), TILE_HEIGHT);
    }
    
    public static TileSprite GetSpriteByTileData(int index, bool hidden = false, bool flagged = false, bool clickedBomb = false, bool badFlagged = false, bool showHeld = false)
    {
        if (hidden)
        {
            if (showHeld)
                return emptySprite;

            if (flagged)
            {
                if (index != BOMB_INDEX && badFlagged)
                {
                    return badFlagSprite;
                }

                return flagSprite;
            }

            return hiddenSprite;
        }

        if (clickedBomb)
            return clickedBombSprite;

        switch (index)
        {
            case <= BOMB_INDEX:
                return bombSprite;
            case 0:
                return emptySprite;
        }

        return new TileSprite(Globals.MainSpriteSheet, 0 + MathHelper.Clamp(index - 1, 0, 7), TILE_HEIGHT);
    }
}