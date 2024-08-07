using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Entities;

namespace Minesweeper;

public class Game1 : Game
{
    private const string GAME_TITLE = "Minesweeper";
    
    private const string SPRITESHEET_ASSET_NAME = "minesweeper_spritesheet";
    
    public const int WINDOW_WIDTH = 324;
    public const int WINDOW_HEIGHT = 464;
    
    public const int DISPLAY_ZOOM_FACTOR = 2;
    
    private const int GRID_X_OFFSET = 0;
    private const int GRID_Y_OFFSET = 70;

    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    
    private Texture2D spriteSheetTexture;
    
    private Matrix transformMatrix = Matrix.Identity * Matrix.CreateScale(DISPLAY_ZOOM_FACTOR, DISPLAY_ZOOM_FACTOR, 1);

    private GameGrid game_grid;
    
    private EntityManager entityManager;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        entityManager = new EntityManager();
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
        
        game_grid = new GameGrid(spriteSheetTexture);
        game_grid.Initialize(GRID_X_OFFSET, GRID_Y_OFFSET);

        foreach (MineTile tile in game_grid.Grid)
        {
            entityManager.AddEntity(tile);
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        entityManager.UpdateEntities(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.White);
        
        spriteBatch.Begin(samplerState:SamplerState.PointClamp, transformMatrix: transformMatrix);
        
        entityManager.DrawEntities(spriteBatch, gameTime);
        
        spriteBatch.End();

        base.Draw(gameTime);
    }
}