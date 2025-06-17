using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Entities;
using Minesweeper.System.Input.Global;

namespace Minesweeper.DataHolders;

public sealed class GridTile(Main game, Vector2 position, int width, int height, int xIndex, int yIndex)
    : Clickable(game, position,
        width, height)
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
    public int Index { get; set; } = 0;
    public bool Hidden { get; set; } = true;
    public bool ShowHeld { get; set; }
    public bool Flagged { get; set; } = false;
    public bool IsBadFlagged { get; set; }
    public bool ClickedBomb { get; set; }
    public event EventHandler<OnClickEventArgs> ClickEvent;
    public event EventHandler<OnFlagEventArgs> FlagEvent;
    public event EventHandler RevealEvent;

    public int xIndex = xIndex;
    public int yIndex = yIndex;

    public Vector2 gridPosition = new(xIndex, yIndex);

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
                if(MouseManager.WasReleased(PointerAction.Primary))
                    game.GameState = GameState.Playing;
                
                ShowHeld = false;
            }
        }
        
        base.Update(gameTime);
    }


    protected override void OnPrimaryAction(Vector2 position, PointerState state)
    {
        if (!InBounds()) return;
        if (state != PointerState.Released) return;
        
        if (game.GameState is GameState.Lose or GameState.Win) return;

        SendToRemote();
        
        OnClickEvent(PointerAction.Primary);
    }
    
    protected override void OnSecondaryAction(Vector2 position, PointerState state)
    {
        if (!InBounds()) return;
        if (state != PointerState.Down) return;
        
        if (game.GameState is GameState.Lose or GameState.Win) return;
        
        SendToRemote();
        
        OnClickEvent(PointerAction.Secondary);
    }
    
    public void Flag()
    {
        Flagged = !Flagged;
        OnFlagEvent(Flagged);
    }

    public void Reveal()
    {
        if (Flagged) return;
        
        if (Hidden)
            OnRevealEvent();
    }


    private void OnClickEvent(PointerAction action)
    {
        EventHandler<OnClickEventArgs> handler = ClickEvent;
        handler?.Invoke(this, new OnClickEventArgs(action));
    }

    private void OnRevealEvent()
    {
        EventHandler handler = RevealEvent;
        handler?.Invoke(this, EventArgs.Empty);
    }

    private void OnFlagEvent(bool flagged)
    {
        EventHandler<OnFlagEventArgs> handler = FlagEvent;
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
        return GetSpriteByTileData(tile.Index, tile.Hidden, tile.Flagged, tile.ClickedBomb, tile.IsBadFlagged, tile.ShowHeld);
    }
    
    public static TileSprite GetSpriteByTileData(int index, bool hidden = false, bool flagged = false, bool clickedBomb = false, bool badFlagged = false, bool showHeld = false)
    {
        if (hidden)
        {
            if (showHeld)
                return emptySprite;

            if (!flagged) return hiddenSprite;
            if (index != BOMB_INDEX && badFlagged)
            {
                return badFlagSprite;
            }

            return flagSprite;

        }

        if (clickedBomb)
            return clickedBombSprite;

        return index switch
        {
            <= BOMB_INDEX => bombSprite,
            0 => emptySprite,
            _ => new TileSprite(Globals.MainSpriteSheet, 0 + MathHelper.Clamp(index - 1, 0, 7), TILE_HEIGHT)
        };
    }
}