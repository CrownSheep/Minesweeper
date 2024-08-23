using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Entities;
using Minesweeper.System;

namespace Minesweeper.DataHolders;

public class GridTile : Clickable
{
    public int Index { get; private set; }
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
        Index = MathHelper.Clamp(index, GridManager.BOMB_INDEX, 8);
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
                if(MouseInputManager.WasReleased(MouseButtons.Left))
                    game.GameState = GameState.Playing;
                
                ShowHeld = false;
            }
        }
        
        base.Update(gameTime);
    }


    protected override void OnLeftMouseClick()
    {
        if(game.GameState != GameState.Lose && game.GameState != GameState.Win)
            OnClickEvent(MouseButtons.Left);
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
        return Index == GridManager.BOMB_INDEX;
    }
    
    public bool IsEmpty()
    {
        return Index == GridManager.EMPTY_INDEX;
    }
}