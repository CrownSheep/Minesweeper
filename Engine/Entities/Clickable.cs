using System;
using Microsoft.Xna.Framework;
using Minesweeper.System.Input.Global;
using Minesweeper.System.Input.Pointer.Remote;

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

    protected bool InBounds() => PointerInput.Inside(Bounds);

    protected void SendToRemote()
    {
        PointerSnapshot[] scaledPointerSnapshots = PointerInput.GetPointerSnapshots();
        for (int i = 0; i < scaledPointerSnapshots.Length; i++)
        {
            PointerSnapshot snapshot = scaledPointerSnapshots[i];
            Vector2 position = snapshot.Position;
            float scale = Main.Environment == GameEnvironments.Desktop
                ? Main.WindowDisplayMode == Main.DisplayMode.Default
                    ? Main.DESKTOP_DEFAULT_ZOOM_FACTOR
                    : Main.DISPLAY_ZOOM_FACTOR
                : Main.MOBILE_DEFAULT_ZOOM_FACTOR;
            position /= scale;
            PointerSnapshot scaledSnapshot = new PointerSnapshot(position, snapshot.Action, snapshot.State);
            scaledPointerSnapshots[i] = scaledSnapshot;
        }
        
        Main.client.SendInput(scaledPointerSnapshots);
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