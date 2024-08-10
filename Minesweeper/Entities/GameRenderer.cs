using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.DataHolders;
using Minesweeper.Extensions;
using Minesweeper.NineSliceSystem;
using Minesweeper.System;

namespace Minesweeper.Entities;

public class GameRenderer : IGameEntity
{
    private const int TEXTURE_COORDS_NUMBER_X = 0;
    private const int TEXTURE_COORDS_NUMBER_Y = 0;

    private const int TEXTURE_COORDS_NUMBER_WIDTH = 13;
    private const int TEXTURE_COORDS_NUMBER_HEIGHT = 23;

    private const int GRID_X_OFFSET = 0;

    public int DrawOrder => 100;

    private Texture2D spriteSheet;

    private GridManager gridManager;

    private TopSectionManager topSectionManager;

    private TopSectionFrame topSectionFrame;
    private GridFrame gridFrame;

    public GameRenderer(Game1 game,Texture2D spriteSheet)
    {
        this.spriteSheet = spriteSheet;
        gridManager = new GridManager(game, this.spriteSheet);
        topSectionFrame = new TopSectionFrame(0, 0, Game1.WINDOW_WIDTH, 56);
        gridFrame = new GridFrame(0, topSectionFrame.Height - 10, GridManager.COLUMNS * 18 + 20, GridManager.ROWS * 18 + 20);
        gridManager.Initialize(topSectionFrame.X + 10, topSectionFrame.Height);
        topSectionManager = new TopSectionManager(gridManager);
    }

    public void Update(GameTime gameTime)
    {
        topSectionManager.timer.Tick(gameTime);

        if (KeyboardInputManager.WasJustPressed(Keys.R))
        {
            gridManager.ResetBoard();
        }

        if (KeyboardInputManager.WasJustPressed(Keys.T))
        {
            topSectionManager.ResetTime();
        }

        foreach (TileManager tile in gridManager.Grid)
        {
            tile.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        topSectionFrame.Draw(spriteBatch, spriteSheet);
        gridFrame.Draw(spriteBatch, spriteSheet);

        spriteBatch.DrawRectangle(
            new Rectangle(topSectionFrame.X + 10, topSectionFrame.Y + 10, topSectionFrame.Width - 20, topSectionFrame.Height - 20),
            Color.Gray);

        DrawNumber(spriteBatch, topSectionManager.ElapsedSeconds, topSectionFrame.X + 16, topSectionFrame.Y + 16);
        DrawNumber(spriteBatch, topSectionManager.LeftFlags, topSectionFrame.Width - 16 - TEXTURE_COORDS_NUMBER_WIDTH * 3, topSectionFrame.Y + 16);

        foreach (TileManager tile in gridManager.Grid)
        {
            tile.Draw(spriteBatch, gameTime);
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