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

namespace Minesweeper.DataHolders;

public class GameStateManager : Clickable
{
    public const int SPRITE_WIDTH = 24;
    public const int SPRITE_HEIGHT = 24;
    
    private GameStateSprite PlayingSprite => new(0);
    
    private GameStateSprite HeldPlayingSprite => new(1);
    private GameStateSprite HeldSprite => new(2);
    private GameStateSprite WinSprite => new(3);
    private GameStateSprite LoseSprite => new(4);

    private Action restartAction;
    
    public GameStateManager(Game1 game, Vector2 position, int width, int height, Action restartAction) : base(game, position, width, height)
    {
        this.restartAction = restartAction;
    }

    protected override void OnLeftMouseClick()
    {
        game.LoadGameWithConfig(game.Config);
        game.GameState = GameState.Playing;
        restartAction.Invoke();
        ParticleManager.SpawnInCircle(new PhysicsParticle
        {
            Sprite = new Sprite(Globals.TransparentSpriteSheet, 4, 27, 17, 17),
        }, new Vector2(Position.X + Width / 4 - 0.3f, Position.Y + Height / 4), 0);
    }

    public GameStateSprite GetSpriteByGameState(GameState state)
    {
        if (CanInteract() && MouseManager.IsCurrently(ButtonState.Pressed, MouseButtons.Left))
        {
            return HeldPlayingSprite;
        }
        
        switch (state)
        {
            case GameState.Held:
                return HeldSprite;
            case GameState.Win:
                return WinSprite;
            case GameState.Lose:
                return LoseSprite;
        }

        return PlayingSprite;
    }

}