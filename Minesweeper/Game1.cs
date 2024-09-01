using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.DataHolders;
using Minesweeper.Entities;
using Minesweeper.Graphics;
using Minesweeper.Particles;
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
    private Texture2D transSpriteSheetTexture;

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

        Globals.Content = Content;
    }


    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.SpriteBatch = spriteBatch;
        spriteSheetTexture = Content.Load<Texture2D>(SPRITESHEET_ASSET_NAME);
        Globals.MainSpriteSheet = spriteSheetTexture;
        transSpriteSheetTexture = MakeGrayPixelsTransparent(spriteSheetTexture);
        Globals.TransparentSpriteSheet = transSpriteSheetTexture;

        LoadGameWithConfig(DefaultConfig);
        gameManager = new GameManager(this, spriteSheetTexture, Config);
    }

    private Texture2D MakeGrayPixelsTransparent(Texture2D texture)
    {
        Color[] pixels = new Color[texture.Width * texture.Height];
        texture.GetData(pixels);

        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = pixels[i];
            if (IsGray(pixel))
            {
                pixels[i] = Color.Transparent;
            }
        }

        Texture2D newTexture = new Texture2D(GraphicsDevice, texture.Width, texture.Height);
        newTexture.SetData(pixels);
        return newTexture;
    }

    private bool IsGray(Color color)
    {
        return color.R == color.G && color.G == color.B && color is not { R: 0, G: 0, B: 0 };
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

        spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix);

        gameManager.Draw(spriteBatch, gameTime);
        ParticleManager.Draw(spriteBatch);

        spriteBatch.End();

        base.Draw(gameTime);
    }

    private void RefreshWindowDimensions()
    {
        graphics.PreferredBackBufferWidth = (int)(WindowWidth * ZoomFactor);
        graphics.PreferredBackBufferHeight = (int)(WindowHeight * ZoomFactor);
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