using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.DataHolders;
using Minesweeper.Entities;
using Minesweeper.Particles;
using Minesweeper.System.Input.Global;
using Minesweeper.System.Input.Keyboard;

namespace Minesweeper;

public class Main : Game
{
    public enum DisplayMode
    {
        Default,
        Zoomed
    }

    private const string GAME_TITLE = "Minesweeper";

    private const string SPRITESHEET_ASSET_NAME = "minesweeper_spritesheet";

    private const int DISPLAY_ZOOM_FACTOR = 3;
    private const int DESKTOP_DEFAULT_ZOOM_FACTOR = 2;
    private const int ANDROID_DEFAULT_ZOOM_FACTOR = 6;

    public int WindowWidth => Config.width * GridTile.TILE_WIDTH + 20;
    public int WindowHeight => 56 + Config.height * GridTile.TILE_HEIGHT + 10;
    
    public static Random Random { get; private set; }
    public static int Seed { get; private set;}

    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch = null!;

    private Texture2D spriteSheetTexture = null!;
    private Texture2D transSpriteSheetTexture = null!;
    
    private SpriteFont font = null!;
    
    public static TCPClient client;
    
    public static IAndroidService androidService;

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

    private GameManager gameManager = null!;

    private GameConfig DefaultConfig
    {
        get
        {
            bool vertical = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width <
                            GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            return Environment == GameEnvironments.Desktop
                ?  new GameConfig(9,
                    16,
                    21, false)
                : new GameConfig(vertical ? 9 : 16,
                    vertical ? 16 : 9,
                    21, false);
        }
    }

    public GameConfig Config { get; private set; } = null!;
    public GameState GameState { get; set; }
    public GameEnvironments Environment { get; set; }
    public DisplayMode WindowDisplayMode { get; set; } = DisplayMode.Default;

    public int ZoomFactor => WindowDisplayMode == DisplayMode.Default
        ? Environment == GameEnvironments.Desktop ? DESKTOP_DEFAULT_ZOOM_FACTOR : ANDROID_DEFAULT_ZOOM_FACTOR
        : DISPLAY_ZOOM_FACTOR;

    public Main(GameEnvironments environment, IAndroidService? service = null)
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        androidService = service ?? new IncompatibleAndroidService();

        Environment = environment;
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        client = new TCPClient();
        
        Window.Title = GAME_TITLE;

        graphics.IsFullScreen = Environment == GameEnvironments.Mobile;
        RefreshWindowDimensions();
        graphics.SynchronizeWithVerticalRetrace = true;
        graphics.SupportedOrientations = DisplayOrientation.Portrait;
        graphics.ApplyChanges();

        Globals.Content = Content;
        
        Seed = global::System.Environment.TickCount;
        Random = new Random(Seed);
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
        
        gameManager.Update(gameTime);
        
        MouseManager.Update();
        TouchManager.Update();
        PointerInput.Update();
        KeyboardManager.Update();
        
        if (KeyboardManager.WasKeyDown(Keys.F12))
        {
            ToggleDisplayMode();
        }

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