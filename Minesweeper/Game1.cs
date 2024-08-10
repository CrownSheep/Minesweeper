using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.DataHolders;
using Minesweeper.Entities;

namespace Minesweeper;

public class Game1 : Game
{
    private const string GAME_TITLE = "Minesweeper";

    private const string SPRITESHEET_ASSET_NAME = "minesweeper_spritesheet";

    public const int WINDOW_WIDTH = GridManager.COLUMNS * 18 + 20;
    public const int WINDOW_HEIGHT = 56 + GridManager.ROWS * 18 + 10;

    public const int DISPLAY_ZOOM_FACTOR = 3;

    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private Texture2D spriteSheetTexture;

    private Matrix transformMatrix = Matrix.Identity * Matrix.CreateScale(DISPLAY_ZOOM_FACTOR, DISPLAY_ZOOM_FACTOR, 1);
    
    private GameRenderer gameRenderer;

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

        graphics.PreferredBackBufferWidth = WINDOW_WIDTH * DISPLAY_ZOOM_FACTOR;
        graphics.PreferredBackBufferHeight = WINDOW_HEIGHT * DISPLAY_ZOOM_FACTOR;
        graphics.SynchronizeWithVerticalRetrace = true;
        graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        spriteSheetTexture = Content.Load<Texture2D>(SPRITESHEET_ASSET_NAME);

        gameRenderer = new GameRenderer(this, spriteSheetTexture);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        gameRenderer.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);
        
        gameRenderer.Draw(spriteBatch, gameTime);

        spriteBatch.End();

        base.Draw(gameTime);
    }
}