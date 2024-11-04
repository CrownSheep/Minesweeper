using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Minesweeper.DataHolders;
using Minesweeper.Entities;
using Minesweeper.Extensions;
using Minesweeper.Particles;
using Minesweeper.System;
using Minesweeper.System.Input.Keyboard;
using Minesweeper.System.Input.Mouse;
using MonoGame.Framework.Utilities;
using Swipe.Android.System.Input.Touch;

namespace Minesweeper;

public class Main : Game
{
    public enum DisplayMode
    {
        Default,
        Zoomed
    }

    public interface IAndroidService
    {
        void Vibrate(long milliseconds, int amplitude = 1);
        void ConsoleLog(string? prefix, string message);
    }

    private const string GAME_TITLE = "Minesweeper";

    private const string SPRITESHEET_ASSET_NAME = "minesweeper_spritesheet";

    private const int DISPLAY_ZOOM_FACTOR = 3;
    private const int DESKTOP_DEFAULT_ZOOM_FACTOR = 2;
    private const int ANDROID_DEFAULT_ZOOM_FACTOR = 6;

    public int WindowWidth => Config.width * 18 + 20;
    public int WindowHeight => 56 + Config.height * 18 + 10;

    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    private Texture2D spriteSheetTexture;
    private Texture2D transSpriteSheetTexture;
    private SpriteFont font;

    public Matrix TransformMatrix
    {
        get =>
            transformMatrix ?? Matrix.Identity *
            Matrix.CreateScale(
                Environment == GameEnvironments.Desktop
                    ? DESKTOP_DEFAULT_ZOOM_FACTOR
                    : ANDROID_DEFAULT_ZOOM_FACTOR - 0.06f,
                Environment == GameEnvironments.Desktop
                    ? DESKTOP_DEFAULT_ZOOM_FACTOR
                    : ANDROID_DEFAULT_ZOOM_FACTOR - 0.02f, 1);
        set => transformMatrix = value;
    }

    private Matrix? transformMatrix;

    private GameManager gameManager;

    GameConfig DefaultConfig
    {
        get
        {
            bool vertical = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width <
                            GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            return Environment == GameEnvironments.Desktop
                ? new GameConfig(12, 12, 24, false)
                : new GameConfig(vertical ? 9 : 16,
                    vertical ? 16 : 9,
                    21, false);
        }
    }

    public GameConfig Config { get; private set; }
    public GameState GameState { get; set; }
    public GameEnvironments Environment { get; set; }
    public DisplayMode WindowDisplayMode { get; set; } = DisplayMode.Default;

    public int ZoomFactor => WindowDisplayMode == DisplayMode.Default
        ? Environment == GameEnvironments.Desktop ? DESKTOP_DEFAULT_ZOOM_FACTOR : ANDROID_DEFAULT_ZOOM_FACTOR
        : DISPLAY_ZOOM_FACTOR;

    public Main(GameEnvironments env = GameEnvironments.Desktop, IAndroidService androidService = null)
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        if (androidService != null)
            Android.Service = androidService;
        Environment = env;
    }

    protected override void Initialize()
    {
        base.Initialize();

        Window.Title = GAME_TITLE;

        graphics.IsFullScreen = Environment == GameEnvironments.Android;
        RefreshWindowDimensions();
        graphics.SynchronizeWithVerticalRetrace = true;
        graphics.SupportedOrientations = DisplayOrientation.Portrait;
        graphics.ApplyChanges();

        Globals.Content = Content;
    }


    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.SpriteBatch = spriteBatch;
        spriteSheetTexture = Content.Load<Texture2D>(SPRITESHEET_ASSET_NAME);
        Globals.MainSpriteSheet = spriteSheetTexture;
        transSpriteSheetTexture = Content.Load<Texture2D>("transparent_" + SPRITESHEET_ASSET_NAME);
        Globals.TransparentSpriteSheet = transSpriteSheetTexture;
        font = Content.Load<SpriteFont>("font");

        LoadGameWithConfig(DefaultConfig);
        gameManager = new GameManager(this, spriteSheetTexture, Config);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        Globals.Update(gameTime);
        ParticleManager.Update(gameTime);

        MouseManager.Update();
        KeyboardManager.Update();
        TouchManager.Update();

        if (KeyboardManager.WasKeyDown(Keys.F12))
        {
            ToggleDisplayMode();
        }

        gameManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);

        spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: TransformMatrix);

        gameManager.Draw(spriteBatch, gameTime);
        ParticleManager.Draw(spriteBatch);

        spriteBatch.End();

        // spriteBatch.Begin(blendState: BlendState.Additive, samplerState: SamplerState.PointClamp, transformMatrix: TransformMatrix);
        // spriteBatch.DrawString(font, GAME_TITLE, Vector2.Zero, Color.White);
        // spriteBatch.End();


        base.Draw(gameTime);
    }

    private void RefreshWindowDimensions()
    {
        graphics.PreferredBackBufferWidth = Environment == GameEnvironments.Desktop
            ? WindowWidth * ZoomFactor
            : GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        graphics.PreferredBackBufferHeight = Environment == GameEnvironments.Desktop
            ? WindowHeight * ZoomFactor
            : GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
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
            TransformMatrix = Matrix.Identity * Matrix.CreateScale(DISPLAY_ZOOM_FACTOR, DISPLAY_ZOOM_FACTOR, 1);
        }
        else
        {
            WindowDisplayMode = DisplayMode.Default;
            graphics.PreferredBackBufferWidth = WindowWidth * DESKTOP_DEFAULT_ZOOM_FACTOR;
            graphics.PreferredBackBufferHeight = WindowHeight * DESKTOP_DEFAULT_ZOOM_FACTOR;
            TransformMatrix = Matrix.Identity *
                              Matrix.CreateScale(DESKTOP_DEFAULT_ZOOM_FACTOR, DESKTOP_DEFAULT_ZOOM_FACTOR, 1);
        }

        graphics.ApplyChanges();
    }
}