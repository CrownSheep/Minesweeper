using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.DataHolders;
using Minesweeper.Extensions;
using Minesweeper.System.Input.Keyboard;

namespace Minesweeper.Entities;

public class GameManager
{
    private const int TEXTURE_COORDS_NUMBER_X = 0;
    private const int TEXTURE_COORDS_NUMBER_Y = 0;

    private const int TEXTURE_COORDS_NUMBER_WIDTH = 13;
    private const int TEXTURE_COORDS_NUMBER_HEIGHT = 23;
    
    private Texture2D spriteSheet;

    private GridManager gridManager;

    private TopSectionManager topSectionManager;

    private TopSectionFrame topSectionFrame;

    private GridFrame gridFrame;

    private GameStateManager gameStateManager;
    
    private Game1 game;

    public GameManager(Game1 game, Texture2D spriteSheet, GameConfig config)
    {
        this.game = game;
        this.spriteSheet = spriteSheet;
        topSectionFrame = new TopSectionFrame(0, 0, game.WindowWidth, 56);
        gridManager = new GridManager(game, new Vector2(topSectionFrame.X + 10, topSectionFrame.Height));
        gridManager.LoseEvent += OnLose;
        gridManager.WinEvent += OnWin;
        gridFrame = new GridFrame(0, topSectionFrame.Height - 10, game.WindowWidth, game.WindowHeight - 56 + 10);
        gridManager.Initialize(config);
        topSectionManager = new TopSectionManager(game, gridManager);
        gameStateManager = new GameStateManager(game, new Vector2(game.WindowWidth / 2 - GameStateManager.SPRITE_WIDTH / 2, topSectionFrame.Y + 16),
            GameStateManager.SPRITE_WIDTH, GameStateManager.SPRITE_HEIGHT, topSectionManager.ResetTime);
    }

    public void OnLose(object sender, EventArgs args)
    {
        topSectionManager.timer.setPaused(true);
    }
    
    public void OnWin(object sender, EventArgs args)
    {
        topSectionManager.timer.setPaused(true);
    }

    public void Update(GameTime gameTime)
    {
        topSectionManager.timer.Tick(gameTime);
        topSectionManager.Update();

        gameStateManager.Update(gameTime);

        foreach (GridTile tile in gridManager.Grid)
        {
            tile.Update(gameTime);
        }
        gridManager.timer.Tick(gameTime);

        if (KeyboardManager.WasKeyDown(Keys.W))
        {
            gridManager.Win();
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        topSectionFrame.Draw(spriteBatch, spriteSheet);
        gridFrame.Draw(spriteBatch, spriteSheet);

        spriteBatch.DrawRectangle(
            new Rectangle(topSectionFrame.X + 10, topSectionFrame.Y + 10, topSectionFrame.Width - 20,
                topSectionFrame.Height - 20),
            Color.Gray);

        if (gridManager.Grid.GetLength(0) >= 7)
        {
            DrawNumber(spriteBatch, topSectionManager.ElapsedSeconds, topSectionFrame.X + 16, topSectionFrame.Y + 16);
            DrawNumber(spriteBatch, topSectionManager.LeftFlags,
                topSectionFrame.Width - 16 - TEXTURE_COORDS_NUMBER_WIDTH * 3, topSectionFrame.Y + 16);
        }

        gameStateManager.GetSpriteByGameState(game.GameState).Draw(spriteBatch, spriteSheet, gameStateManager.Position);

        foreach (GridTile tile in gridManager.Grid)
        {
            GridTile.GetSpriteByTile(tile).Draw(spriteBatch, tile.Position);
        }
    }

    private void DrawNumber(SpriteBatch spriteBatch, int number, float startPosX, float y)
    {
        int[] scoreDigits = SplitDigits(Math.Abs(number));

        float posX = startPosX;

        foreach (int digit in scoreDigits)
        {
            Rectangle textureCoords = GetDigitTextureBounds(digit);

            Vector2 screenPos = new Vector2(posX, y);

            spriteBatch.Draw(spriteSheet, screenPos, textureCoords, Color.White);

            posX += TEXTURE_COORDS_NUMBER_WIDTH;
        }

        if (number < 0)
        {
            const int minusPosX = TEXTURE_COORDS_NUMBER_X + 10 * TEXTURE_COORDS_NUMBER_WIDTH;

            spriteBatch.Draw(spriteSheet, new Vector2(startPosX, y),
                new Rectangle(minusPosX, TEXTURE_COORDS_NUMBER_Y, TEXTURE_COORDS_NUMBER_WIDTH,
                    TEXTURE_COORDS_NUMBER_HEIGHT), Color.White);
        }
    }

    private int[] SplitDigits(int input)
    {
        string inputStr = input.ToString().PadLeft(3, '0');

        int[] result = new int[inputStr.Length];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = (int)char.GetNumericValue(inputStr[i]);
        }

        return result;
    }

    private Rectangle GetDigitTextureBounds(int digit)
    {
        if (digit < 0 || digit > 9)
            throw new ArgumentOutOfRangeException(nameof(digit), "The value of digit must be between 0 and 9.");

        int posX = TEXTURE_COORDS_NUMBER_X + digit * TEXTURE_COORDS_NUMBER_WIDTH;
        int posY = TEXTURE_COORDS_NUMBER_Y;

        return new Rectangle(posX, posY, TEXTURE_COORDS_NUMBER_WIDTH, TEXTURE_COORDS_NUMBER_HEIGHT);
    }
}