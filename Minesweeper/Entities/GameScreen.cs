using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.DataHolders;
using Minesweeper.GameElements;
using Minesweeper.System;

namespace Minesweeper.Entities;

public class GameScreen : IGameEntity
{
    
    private const int TEXTURE_COORDS_NUMBER_X = 0;
    private const int TEXTURE_COORDS_NUMBER_Y = 0;

    private const int TEXTURE_COORDS_NUMBER_WIDTH = 13;
    private const int TEXTURE_COORDS_NUMBER_HEIGHT = 23;
    
    
    private const int GRID_X_OFFSET = 0;
    private const int GRID_Y_OFFSET = 70;
    
    public int DrawOrder => 100;
    public Vector2 Position { get; set; }

    private Texture2D spriteSheet;
    
    private GridManager gridManager;

    private TopSectionManager topSectionManager;

    public GameScreen(Texture2D spriteSheet, Vector2 position)
    {
        this.spriteSheet = spriteSheet;
        Position = position;
        gridManager = new GridManager(this.spriteSheet);
        gridManager.Initialize(GRID_X_OFFSET, GRID_Y_OFFSET);
        topSectionManager = new TopSectionManager(gridManager);
    }
    
    public void Update(GameTime gameTime)
    {
        topSectionManager.timer.Tick(gameTime);

        if (KeyboardInputManager.WasJustPressed(Keys.R))
        {
            gridManager.ResetBoard();
        }
        
        foreach (MineTile tile in gridManager.Grid)
        {
            tile.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        DrawNumber(spriteBatch, topSectionManager.ElapsedSeconds, Position.X);
        DrawNumber(spriteBatch, topSectionManager.LeftFlags, Position.X + 100);
        
        foreach (MineTile tile in gridManager.Grid)
        {
            tile.Draw(spriteBatch, gameTime);
        }
    }
    
    private void DrawNumber(SpriteBatch spriteBatch, int number, float startPosX)
    {
        int[] scoreDigits = SplitDigits(Math.Abs(number));

        float posX = startPosX;

        foreach (int digit in scoreDigits)
        {
            Rectangle textureCoords = GetDigitTextureBounds(digit);

            Vector2 screenPos = new Vector2(posX, Position.Y);

            spriteBatch.Draw(spriteSheet, screenPos, textureCoords, Color.White);

            posX += TEXTURE_COORDS_NUMBER_WIDTH;
        }

        if (number < 0)
        {
            int minusPosX = TEXTURE_COORDS_NUMBER_X + 10 * TEXTURE_COORDS_NUMBER_WIDTH;
            spriteBatch.Draw(spriteSheet, new Vector2(startPosX, Position.Y),
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