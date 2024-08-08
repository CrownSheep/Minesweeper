using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.System;

namespace Minesweeper.Entities;

public class InfoBar : IGameEntity
{
    
    private const int TEXTURE_COORDS_NUMBER_X = 0;
    private const int TEXTURE_COORDS_NUMBER_Y = 0;

    private const int TEXTURE_COORDS_NUMBER_WIDTH = 13;
    private const int TEXTURE_COORDS_NUMBER_HEIGHT = 23;
    
    public int DrawOrder => 100;

    private Timer timer = new Timer(1);
    public Vector2 Position { get; set; }

    private Texture2D spriteSheet;

    private int secondsPassed;
    
    private GameGrid gameGrid;

    public InfoBar(Texture2D spriteSheet, Vector2 position, GameGrid gameGrid)
    {
        this.spriteSheet = spriteSheet;
        Position = position;
        timer.FinishEvent += onSecondIncrement;
        timer.setPaused(true);
        this.gameGrid = gameGrid;
        this.gameGrid.ClickEvent += onClickEvent;
    }
    
    public void Update(GameTime gameTime)
    {
        timer.Tick(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        DrawNumber(spriteBatch, secondsPassed, Position.X);
    }

    
    private void onSecondIncrement(object sender, EventArgs e)
    {
        secondsPassed++;
    }
    
    private void onClickEvent(object sender, EventArgs e)
    {
        MineTile tile = (MineTile) sender;
        OnClickEventArgs clickEventArgs = (OnClickEventArgs) e;
        if (clickEventArgs.button == Mouse.GetState().LeftButton)
        {
            if (!tile.Flagged)
                timer.setPaused(false);

            if (tile.isBomb())
                timer.setPaused(true);
        } else if (clickEventArgs.button == Mouse.GetState().RightButton)
        {
            
        }

    }
    
    private void DrawNumber(SpriteBatch spriteBatch, int number, float startPosX)
    {
        int[] scoreDigits = SplitDigits(number);

        float posX = startPosX;

        foreach (int digit in scoreDigits)
        {
            Rectangle textureCoords = GetDigitTextureBounds(digit);

            Vector2 screenPos = new Vector2(posX, Position.Y);

            spriteBatch.Draw(spriteSheet, screenPos, textureCoords, Color.White);

            posX += TEXTURE_COORDS_NUMBER_WIDTH;
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