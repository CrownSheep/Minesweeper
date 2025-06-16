using Microsoft.Xna.Framework;
using Minesweeper.System.Input.Global;

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

        Vector2 primaryPosition = PointerInput.GetPosition(PointerAction.Primary);
        Vector2 secondaryPosition = PointerInput.GetPosition(PointerAction.Secondary);
        Vector2 tertiaryPosition = PointerInput.GetPosition(PointerAction.Tertiary);
        
        OnPrimaryAction(primaryPosition, PointerInput.GetPointerState(PointerAction.Primary));
        OnSecondaryAction(secondaryPosition, PointerInput.GetPointerState(PointerAction.Secondary));
        OnTertiaryAction(tertiaryPosition, PointerInput.GetPointerState(PointerAction.Tertiary));
    }

    protected bool InBounds()
    {
        return PointerInput.Inside(Bounds);
    }

    protected virtual void OnPrimaryAction(Vector2 position, PointerState state)
    {
        
    }

    protected virtual void OnSecondaryAction(Vector2 position, PointerState state)
    {
        
    }

    protected virtual void OnTertiaryAction(Vector2 position, PointerState state)
    {
        
    }
}