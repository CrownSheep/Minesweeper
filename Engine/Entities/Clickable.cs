using System.Linq;
using Microsoft.Xna.Framework;
using Minesweeper.System;
using Minesweeper.System.Input.Mouse;
using Swipe.Android.System.Input.Touch;

namespace Minesweeper.Entities;

public abstract class Clickable(Main game, Vector2 position, int width, int height, params MouseButton[] instantButtons)
{
    public Vector2 Position { get; } = position;
    public int Width { get; } = width;
    public int Height { get; } = height;

    protected Rectangle Bounds => new((Position * game.ZoomFactor).ToPoint(),
        new Point(Width * game.ZoomFactor, Height * game.ZoomFactor));

    protected readonly Main game = game;

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
            MouseButton.Left => TouchManager.WasReleased(),
            MouseButton.Right => TouchManager.HeldOver(),
            _ => false
        };
        
        return (instantButtons.Contains(button)
            ? MouseManager.WasClicked(button)
            : MouseManager.WasReleased(button)) || mobileClicked;
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