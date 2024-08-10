using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper.Entities;

public abstract class Clickable : IGameEntity
{
    public Vector2 Position { get; }
    public int Width { get; }
    public int Height { get; }
    
    public int DrawOrder { get; }
    
    private Rectangle Bounds => new Rectangle((Position * Game1.DISPLAY_ZOOM_FACTOR).ToPoint(),
        new Point(Width * Game1.DISPLAY_ZOOM_FACTOR, Height * Game1.DISPLAY_ZOOM_FACTOR));
    
    private MouseState previousMouseState;

    private Game1 game;

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
            if (Bounds.Contains(mouseState.Position))
            {
                if (mouseButtonClicked(mouseState.LeftButton, previousMouseState.LeftButton))
                    OnLeftMouseClick();
                if (mouseButtonClicked(mouseState.RightButton, previousMouseState.RightButton))
                    OnRightMouseClick();
                if (mouseButtonClicked(mouseState.MiddleButton, previousMouseState.MiddleButton))
                    OnMiddleMouseClick();
            }
        }

        previousMouseState = mouseState;
    }

    private bool mouseButtonClicked(ButtonState button, ButtonState previousButton)
    {
        return button == ButtonState.Pressed && previousButton != ButtonState.Pressed;
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

    public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
}
