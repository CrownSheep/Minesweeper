using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.DataHolders;
using Minesweeper.Entities;
using Minesweeper.System;
using Minesweeper.System.Input.Keyboard;
using Minesweeper.System.Input.Mouse;

namespace Minesweeper;

public class Game1 : Game
{
    public enum DisplayMode
    {
        Default,
        Zoomed
    }
    
    private const string GAME_TITLE = "Minesweeper";

    private const string SPRITESHEET_ASSET_NAME = "minesweeper_spritesheet";

    private const int DISPLAY_ZOOM_FACTOR = 3;
    private const int DEFAULT_ZOOM_FACTOR = 2;

    public int WindowWidth => Config.width * 18 + 20;
    public int WindowHeight => 56 + Config.height * 18 + 10;

    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private Texture2D spriteSheetTexture;

    private Matrix transformMatrix = Matrix.Identity * Matrix.CreateScale(DEFAULT_ZOOM_FACTOR, DEFAULT_ZOOM_FACTOR, 1);
    
    private GameManager gameManager;
    GameConfig DefaultConfig => GameConfig.BEGINNER;
    public GameConfig Config { get; private set; }
    public GameState GameState { get; set; }
    public DisplayMode WindowDisplayMode { get; set; } = DisplayMode.Default;
    public float ZoomFactor => WindowDisplayMode == DisplayMode.Default ? DEFAULT_ZOOM_FACTOR : DISPLAY_ZOOM_FACTOR;

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

        graphics.PreferredBackBufferWidth = WindowWidth * DEFAULT_ZOOM_FACTOR;
        graphics.PreferredBackBufferHeight = WindowHeight * DEFAULT_ZOOM_FACTOR;
        graphics.SynchronizeWithVerticalRetrace = true;
        graphics.ApplyChanges();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        spriteSheetTexture = Content.Load<Texture2D>(SPRITESHEET_ASSET_NAME);
        
        LoadGameWithConfig(DefaultConfig);
        gameManager = new GameManager(this, spriteSheetTexture, Config);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        MouseInputManager.Update();
        KeyboardInputManager.Update();
        
        if (KeyboardInputManager.WasKeyDown(Keys.F12))
        {
            ToggleDisplayMode();
        }
        
        if (MouseInputManager.GetScrollWheelValue() != MouseInputManager.GetOldScrollWheelValue())
        {
            Console.WriteLine(MouseInputManager.GetScrolledValue());
        }
        
        gameManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);
        
        gameManager.Draw(spriteBatch, gameTime);

        spriteBatch.End();

        base.Draw(gameTime);
    }

    private void RefreshWindowDimensions()
    {
        graphics.PreferredBackBufferWidth = (int) (WindowWidth * ZoomFactor);
        graphics.PreferredBackBufferHeight = (int) (WindowHeight * ZoomFactor);
        graphics.ApplyChanges();
    }

    public void LoadGameWithConfig(GameConfig config)
    {
        Config = config;
        gameManager = new GameManager(this, spriteSheetTexture, Config);
        RefreshWindowDimensions();
    }
    
    private void ToggleDisplayMode()
    {
        if (WindowDisplayMode == DisplayMode.Default)
        {
            WindowDisplayMode = DisplayMode.Zoomed;
            graphics.PreferredBackBufferWidth = WindowWidth * DISPLAY_ZOOM_FACTOR;
            graphics.PreferredBackBufferHeight = WindowHeight * DISPLAY_ZOOM_FACTOR;
            transformMatrix = Matrix.Identity * Matrix.CreateScale(DISPLAY_ZOOM_FACTOR, DISPLAY_ZOOM_FACTOR, 1);
        }
        else
        {
            WindowDisplayMode = DisplayMode.Default;
            graphics.PreferredBackBufferWidth = WindowWidth * DEFAULT_ZOOM_FACTOR;
            graphics.PreferredBackBufferHeight = WindowHeight * DEFAULT_ZOOM_FACTOR;
            transformMatrix = Matrix.Identity * Matrix.CreateScale(DEFAULT_ZOOM_FACTOR, DEFAULT_ZOOM_FACTOR, 1);
        }
            
        graphics.ApplyChanges();
    }
}