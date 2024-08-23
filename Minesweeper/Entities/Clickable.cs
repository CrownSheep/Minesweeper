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

    private MouseState previousMouseState;

    protected Game1 game;

    protected Clickable(Game1 game, Vector2 position, int width, int height)
    {
        this.game = game;
        Position = position;
        Width = width;
        Height = height;
    }


    public virtual void Update(GameTime gameTime)
    {
        MouseState mouseState = Mouse.GetState();

        if (game.IsActive)
        {
            if (MouseInputManager.Hover(Bounds))
            {
                if(MouseInputManager.IsLeftClicked(true))
                    OnLeftMouseClick();
                if (MouseInputManager.IsRightClicked(false))
                    OnRightMouseClick();
                // if (MouseInputManager.IsMiddleClicked(false))
                //     OnMiddleMouseClick();
            }
        }

        previousMouseState = mouseState;
    }

    protected bool canInteract()
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