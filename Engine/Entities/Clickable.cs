using System.Linq;
using Microsoft.Xna.Framework;
using Minesweeper.System;
using Minesweeper.System.Input.Global;
using Minesweeper.System.Input.Mouse;
using Swipe.Android.System.Input.Touch;

namespace Minesweeper.Entities;

public abstract class Clickable(Main game, Vector2 position, int width, int height)
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
        
        if (!CanInteract()) return;

        if (CheckClick(MouseButton.Left))
            OnLeftMouseClick();
        if (CheckClick(MouseButton.Right))
            OnRightMouseClick();
        if (CheckClick(MouseButton.Middle))
            OnMiddleMouseClick();
    }

    private bool CheckClick(PointerAction action)
    {
        bool mobileClicked = action switch
        {
            MouseButton.Left => TouchManager.WasReleased(),
            MouseButton.Right => TouchManager.HeldOver(),
            _ => false
        };
        
        return (instantButtons.Contains(action)
            ? MouseManager.WasClicked(action)
            : MouseManager.WasReleased(action)) || mobileClicked;
    }

    protected bool CanInteract()
    {
        return game.IsActive && (MouseManager.Inside(Bounds) || TouchManager.Inside(Bounds));
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