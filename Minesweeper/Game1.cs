using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Entities;
using Minesweeper.GameElements;

namespace Minesweeper;

public class Game1 : Game
{
    private const string GAME_TITLE = "Minesweeper";

    private const string SPRITESHEET_ASSET_NAME = "minesweeper_spritesheet";

    public const int WINDOW_WIDTH = 324 * 2;
    public const int WINDOW_HEIGHT = 464 * 2;

    public const int DISPLAY_ZOOM_FACTOR = 3;

    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private Texture2D spriteSheetTexture;

    private Matrix transformMatrix = Matrix.Identity * Matrix.CreateScale(DISPLAY_ZOOM_FACTOR, DISPLAY_ZOOM_FACTOR, 1);

    private GridManager _gridManager;
    private GameScreen _gameScreen;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();

        Window.Title = GAME_TITLE;

        graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
        graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
        graphics.SynchronizeWithVerticalRetrace = true;
        graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        spriteSheetTexture = Content.Load<Texture2D>(SPRITESHEET_ASSET_NAME);

        _gameScreen = new GameScreen(spriteSheetTexture, new Vector2(0, 0));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        _gameScreen.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);
        
        _gameScreen.Draw(spriteBatch, gameTime);

        spriteBatch.End();

        base.Draw(gameTime);
    }
}