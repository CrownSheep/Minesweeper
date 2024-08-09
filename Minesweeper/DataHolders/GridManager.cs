using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Minesweeper.GameElements;

public class GridManager
{
    public const int COLUMNS = 9;
    public const int ROWS = 9;

    public MineTile[,] Grid { get; } = new MineTile[COLUMNS, ROWS];

    private Texture2D spriteSheet;

    private bool initialTile = true;

    public const int BOMB_COUNT = 10;

    private int placedBombs = 0;

    private Random random;

    private List<MineTile> bombTiles = new List<MineTile>();
    private List<MineTile> flagTiles = new List<MineTile>();

    private bool revealedBombs;
    public event EventHandler ClickEvent;
    public event EventHandler FlagEvent;

    public int FlagCount => flagTiles.Count;

    public MineTile this[int xindex, int yindex]
    {
        get { return Grid[xindex, yindex]; }
    }

    public GridManager(Texture2D spriteSheet)
    {
        this.spriteSheet = spriteSheet;
        random = new Random();
    }

    public void Initialize(int xOffset, int yOffset)
    {
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Grid[i, j] = new MineTile(spriteSheet,
                    new Vector2(xOffset + i * MineTile.TILE_WIDTH, yOffset + j * MineTile.TILE_HEIGHT),
                    MineTile.TILE_WIDTH, MineTile.TILE_HEIGHT, i, j);
                Grid[i, j].ClickEvent += OnClickTile;
                Grid[i, j].RevealEvent += OnRevealTile;
                Grid[i, j].FlagEvent += OnFlagTile;
            }
        }
    }

    public void ResetBoard()
    {
        initialTile = true;
        revealedBombs = false;
        bombTiles.Clear();
        flagTiles.Clear();
        foreach (MineTile tile in Grid)
        {
            tile.SetIndex(0);
            tile.ShouldDisplayWrongfulFlagged = false;
            tile.Flagged = false;
            tile.SetHidden(true);
        }
    }

    public void SetBombs(MineTile safeTile, int bombCount)
    {
        int bombsPlaced = 0;
        while (bombsPlaced < bombCount)
        {
            int randX = random.Next(ROWS);
            int randY = random.Next(COLUMNS);
            MineTile randTile = Grid[randX, randY];
            if (!randTile.isBomb() && !InZone(randTile.indexs, safeTile.indexs))
            {
                randTile.SetIndex(-1);
                bombsPlaced++;
                bombTiles.Add(randTile);
            }
        }

        initialTile = false;
    }

    private static bool InZone(Vector2 position, Vector2 safePosition)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (position == new Vector2(safePosition.X + x, safePosition.Y + y))
                {
                    return true;
                }
            }
        }

        return false;
    }

    protected virtual void OnClickEvent(MineTile tile, ButtonState buttonState)
    {
        EventHandler handler = ClickEvent;
        handler?.Invoke(tile, new OnClickEventArgs(buttonState));
    }

    private void OnFlagTile(object sender, EventArgs e)
    {
        MineTile tile = (MineTile)sender;
        OnFlagEventArgs args = (OnFlagEventArgs)e;
        if (args.Flagged)
        {
            flagTiles.Add(tile);
        }
        else
        {
            flagTiles.Remove(tile);
        }
    }

    private void OnRevealTile(object sender, EventArgs e)
    {
        MineTile clickedTile = (MineTile)sender;
        clickedTile.SetHidden(false);
    }

    private void OnClickTile(object sender, EventArgs e)
    {
        MineTile clickedTile = (MineTile)sender;
        ButtonState button = ((OnClickEventArgs)e).Button;
        
        if (button == Mouse.GetState().LeftButton)
        {
            OnClickEvent(clickedTile, button);
            if (!revealedBombs && !clickedTile.Flagged)
            {
                if (initialTile)
                {
                    SetBombs(clickedTile, BOMB_COUNT);
                }

                if (clickedTile.isEmpty())
                {
                    clickedTile.Reveal();
                    clickedTile.SetIndex(AdjacentBombCount(clickedTile));
                    RevealAdjacentEmptyTiles(clickedTile, button);
                }
                else if (clickedTile.isBomb())
                {
                    RevealAllBombs(clickedTile);
                    revealedBombs = true;
                }
            }
        }

        if (button == Mouse.GetState().RightButton)
        {
            if (clickedTile.IsHidden())
                clickedTile.Flag();
        }
    }

    private void RevealAllBombs(MineTile tile)
    {
        tile.SetIndex(-2);
        foreach (MineTile bombTile in bombTiles)
        {
            if (!bombTile.Flagged)
                bombTile.SetHidden(false);
        }

        foreach (MineTile flagTile in flagTiles)
        {
            if (!flagTile.isBomb())
                flagTile.ShouldDisplayWrongfulFlagged = true;
        }
    }

    private void RevealAdjacentEmptyTiles(MineTile clickedTile, ButtonState buttonState)
    {
        int[] positions = { -1, 0, 1 };
        foreach (int xPosition in positions)
        {
            foreach (int yPosition in positions)
            {
                int adjacentX = clickedTile.xIndex + xPosition;
                int adjacentY = clickedTile.yIndex + yPosition;

                if (adjacentX >= 0 && adjacentY >= 0 && adjacentX < ROWS && adjacentY < COLUMNS)
                {
                    MineTile adjacentTile = Grid[adjacentX, adjacentY];
                    if (clickedTile.Index == 0)
                    {
                        if (adjacentTile.Index == 0 && adjacentTile.IsHidden())
                        {
                            adjacentTile.SetIndex(AdjacentBombCount(adjacentTile));
                            adjacentTile.SetHidden(false);
                            OnClickTile(adjacentTile, new OnClickEventArgs(buttonState));
                        }
                    }
                }
            }
        }
    }

    private int AdjacentBombCount(MineTile tile)
    {
        int[] positions = { -1, 0, 1 };
        int bombCount = 0;

        foreach (int xPosition in positions)
        {
            foreach (int yPosition in positions)
            {
                int adjacentX = tile.xIndex + xPosition;
                int adjacentY = tile.yIndex + yPosition;

                if (adjacentX >= 0 && adjacentY >= 0 && adjacentX < ROWS && adjacentY < COLUMNS ||
                    (xPosition == 0 && yPosition == 0))
                {
                    MineTile adjacentTile = Grid[adjacentX, adjacentY];

                    if (adjacentTile.Index == -1)
                    {
                        bombCount++;
                    }
                }
            }
        }

        return bombCount;
    }
}