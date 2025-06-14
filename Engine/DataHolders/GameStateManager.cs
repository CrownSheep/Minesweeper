using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Entities;
using Minesweeper.Extensions;
using Minesweeper.Graphics;
using Minesweeper.Particles;
using Minesweeper.System;
using Minesweeper.System.Input.Mouse;
using Swipe.Android.System.Input.Touch;

namespace Minesweeper.DataHolders;

public class GameStateManager(Main game, Vector2 position, int width, int height, Action restartAction)
    : Clickable(game, position, width, height)
{
    public const int SPRITE_WIDTH = 24;
    public const int SPRITE_HEIGHT = 24;

    protected override void OnLeftMouseClick()
    {
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
        if (CanInteract() && (MouseManager.IsCurrently(ButtonState.Pressed, MouseButton.Left) || TouchManager.HasIndex()))
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