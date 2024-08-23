using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.System;

namespace Minesweeper.Entities;

public abstract class Clickable
{
    public Vector2 Position { get; }
    public int Width { get; }
    public int Height { get; }

    protected Rectangle Bounds => new Rectangle((Position * game.ZoomFactor).ToPoint(),
        new Point((int) (Width * game.ZoomFactor), (int) (Height * game.ZoomFactor)));

    protected readonly Game1 game;
    
    private readonly MouseButtons[] instantButtons;

    protected Clickable(Game1 game, Vector2 position, int width, int height, params MouseButtons[] instantButtons)
    {
        this.game = game;
        Position = position;
        Width = width;
        Height = height;
        this.instantButtons = instantButtons;
    }


    public virtual void Update(GameTime gameTime)
    {
        if (!game.IsActive) return;
        
        if (!MouseInputManager.Hover(Bounds)) return;
        
        if(CheckClick(MouseButtons.Left))
            OnLeftMouseClick();
        if (CheckClick(MouseButtons.Right))
            OnRightMouseClick();
        if (CheckClick(MouseButtons.Middle))
            OnMiddleMouseClick();
    }

    private bool CheckClick(MouseButtons button)
    {
        return instantButtons.Contains(button)
            ? MouseInputManager.WasClicked(button)
            : MouseInputManager.WasReleased(button);
    }

    protected bool CanInteract()
    {
        return game.IsActive && MouseInputManager.Hover(Bounds);
    }

    protected virtual void OnLeftMouseClick()
    {
    }

    protected virtual void OnRightMouseClick()
    {
    }

    protected virtual void OnMiddleMouseClick()
    {
    }
}