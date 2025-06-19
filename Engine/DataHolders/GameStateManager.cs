using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Entities;
using Minesweeper.Graphics;
using Minesweeper.Particles;
using Minesweeper.System.Input.Global;
using Minesweeper.System.Input.Pointer.Remote;

namespace Minesweeper.DataHolders;

public class GameStateManager(Main game, Vector2 position, int width, int height, Action restartAction)
    : Clickable(game, position, width, height)
{
    public const int SPRITE_WIDTH = 24;
    public const int SPRITE_HEIGHT = 24;

    protected override void OnPrimaryAction(Vector2 localPosition, PointerState localState,
        Vector2 remotePosition, PointerState remoteState)
    {
        if (!InBounds() && !RemoteInput.Inside(Bounds)) return;
        
        if (localState != PointerState.Released && remoteState != PointerState.Released) return;
        
        game.LoadGameWithConfig(game.Config);
        game.GameState = GameState.Playing;
        restartAction.Invoke();
        ParticleManager.SpawnInCircle(new PhysicsParticle()
        {
            Sprite = Sprites.Smile
        }, new Vector2(Position.X + Width / 2, Position.Y + Height / 2), 0);
        
        SendInputToRemote();
    }

    public GameStateSprite GetSpriteByGameState(GameState state)
    {
        if (InBounds() && (MouseManager.IsCurrently(ButtonState.Pressed, PointerAction.Primary) || TouchManager.HasFinger()))
        {
            return GameStateSprite.HeldPlayingSprite;
        }

        return state switch
        {
            GameState.Held => GameStateSprite.HeldSprite,
            GameState.Win => GameStateSprite.WinSprite,
            GameState.Lose => GameStateSprite.LoseSprite,
            _ => GameStateSprite.PlayingSprite
        };
    }

}