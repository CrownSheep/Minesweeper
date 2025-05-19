using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.System;
using Minesweeper.System.Input.Mouse;
using Swipe.Android.System.Input.Touch;

namespace Minesweeper.Entities;

public abstract class Clickable
{
    public Vector2 Position { get; }
    public int Width { get; }
    public int Height { get; }

    protected Rectangle Bounds => new Rectangle((Position * game.ZoomFactor).ToPoint(),
        new Point((int)(Width * game.ZoomFactor), (int)(Height * game.ZoomFactor)));

    protected readonly Main game;

    private readonly MouseButton[] instantButtons;

    protected Clickable(Main game, Vector2 position, int width, int height, params MouseButton[] instantButtons)
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

        if (!(MouseManager.Hover(Bounds) || TouchManager.Inside(Bounds))) return;

        if (CheckClick(MouseButton.Left))
            OnLeftMouseClick();
        if (CheckClick(MouseButton.Right))
            OnRightMouseClick();
        if (CheckClick(MouseButton.Middle))
            OnMiddleMouseClick();
    }

    private bool CheckClick(MouseButton button)
    {
        bool mobileClicked = button switch
        {
            MouseButton.Left => TouchManager.WasReleased() && !TouchManager.PutFlagged(),
            MouseButton.Right => TouchManager.HeldOver(),
            _ => false
        };
        return (instantButtons.Contains(button)
            ? MouseManager.WasClicked(button)
            : MouseManager.WasReleased(button)) || (game.Environment == GameEnvironments.Android && mobileClicked);
    }

    protected bool CanInteract()
    {
        return game.IsActive && (MouseManager.Hover(Bounds) || TouchManager.Inside(Bounds));
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