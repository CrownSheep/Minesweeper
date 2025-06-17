using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Entities;
using Minesweeper.Graphics;
using Minesweeper.Particles;
using Minesweeper.System.Input.Global;

namespace Minesweeper.DataHolders;

public class GameStateManager(Main game, Vector2 position, int width, int height, Action restartAction)
    : Clickable(game, position, width, height)
{
    public const int SPRITE_WIDTH = 24;
    public const int SPRITE_HEIGHT = 24;

    protected override void OnPrimaryAction(Vector2 position, PointerState state)
    {
        if (!InBounds()) return;
        if (state != PointerState.Released) return;

        SendToRemote();
        
        game.LoadGameWithConfig(game.Config);
        game.GameState = GameState.Playing;
        restartAction.Invoke();
        ParticleManager.SpawnInCircle(new PhysicsParticle()
        {
            Sprite = new Sprite(Globals.TransparentSpriteSheet, 4, 27, 17, 17),
        }, new Vector2(Position.X + Width / 2, Position.Y + Height / 2), 0);
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