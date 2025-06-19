using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Minesweeper.System.Input;
using Minesweeper.System.Input.Global;
using Minesweeper.System.Input.Pointer.Remote;

namespace Minesweeper.Entities;

public abstract class Clickable(Main game, Vector2 position, int width, int height)
{
    public Vector2 Position { get; } = position;
    public int Width { get; } = width;
    public int Height { get; } = height;

    protected Rectangle Bounds => new((Position * Main.ZoomFactor).ToPoint(),
        new Point(Width * Main.ZoomFactor, Height * Main.ZoomFactor));

    protected readonly Main game = game;

    public virtual void Update(GameTime gameTime)
    {
        if (!game.IsActive) return;

        Vector2 localPrimaryPosition = PointerInput.GetPosition(PointerAction.Primary);
        Vector2 localSecondaryPosition = PointerInput.GetPosition(PointerAction.Secondary);
        Vector2 localTertiaryPosition = PointerInput.GetPosition(PointerAction.Tertiary);
        
        Vector2 remotePrimaryPosition = RemoteInput.GetPosition(PointerAction.Primary);
        Vector2 remoteSecondaryPosition = RemoteInput.GetPosition(PointerAction.Secondary);
        Vector2 remoteTertiaryPosition = RemoteInput.GetPosition(PointerAction.Tertiary);
        
        OnPrimaryAction(localPrimaryPosition, PointerInput.GetPointerState(PointerAction.Primary),
            remotePrimaryPosition, RemoteInput.GetPointerState(PointerAction.Primary));
        OnSecondaryAction(localSecondaryPosition, PointerInput.GetPointerState(PointerAction.Secondary),
            remoteSecondaryPosition, RemoteInput.GetPointerState(PointerAction.Secondary));
        OnTertiaryAction(localTertiaryPosition, PointerInput.GetPointerState(PointerAction.Tertiary),
            remoteTertiaryPosition, RemoteInput.GetPointerState(PointerAction.Tertiary));
    }

    protected bool InBounds() => PointerInput.Inside(Bounds);

    protected void SendInputToRemote()
    {
        PointerSnapshot[] snapshots = PointerInput.GetPointerSnapshots();

        for (int i = 0; i < snapshots.Length; i++)
        {
            var s = snapshots[i];
            var scaledPos = ScreenSpaceUtils.ToCommonPosition(s.Position, true);
            snapshots[i] = new PointerSnapshot(scaledPos, s.Action, s.State);
        }
        
        Main.client.SendMessage(MessageType.PointerInput, snapshots);
        TakeOffRemotePointer();
    }

    private void TakeOffRemotePointer()
    {
        PointerSnapshot[] snapshots = PointerInput.GetPointerSnapshots();

        for (int i = 0; i < snapshots.Length; i++)
        {
            var s = snapshots[i];
            snapshots[i] = new PointerSnapshot(Vector2.Zero, s.Action, PointerState.None);
        }
        
        RemoteInput.UpdateSnapshots(snapshots);
    }

    protected virtual void OnPrimaryAction(Vector2 localPosition, PointerState localState,
        Vector2 remotePosition, PointerState remoteState)
    {
        
    }

    protected virtual void OnSecondaryAction(Vector2 localPosition, PointerState localState,
        Vector2 remotePosition, PointerState remoteState)
    {
        
    }

    protected virtual void OnTertiaryAction(Vector2 localPosition, PointerState localState,
        Vector2 remotePosition, PointerState remoteState)
    {
        
    }
}