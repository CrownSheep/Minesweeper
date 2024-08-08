using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Minesweeper.Entities;

namespace Minesweeper;

public class GameGrid
{
    public const int COLUMNS = 9;
    public const int ROWS = 9;

    public MineTile[,] Grid { get; } = new MineTile[COLUMNS, ROWS];

    private Texture2D spriteSheet;

    private bool initialTile = true;

    private const int BOMB_COUNT = 10;

    private int placedBombs = 0;

    private Random random;
    
    private List<MineTile> bombTiles = new List<MineTile>();
    private List<MineTile> flagTiles = new List<MineTile>();
    
    public event EventHandler ClickEvent;

    public MineTile this[int xindex, int yindex]
    {
        get { return Grid[xindex, yindex]; }
    }

    public GameGrid(Texture2D spriteSheet)
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
                Grid[i, j].FlagEvent += OnFlagTile;
            }
        }
    }

    public void SetBoard(MineTile safeTile, int bombCount)
    {
        int bombsPlaced = 0;
        while (bombsPlaced <= bombCount)
        {
            int randX = random.Next(ROWS);
            int randY = random.Next(COLUMNS);
            MineTile randTile = Grid[randX, randY];
            if (!(randTile.Index == -1 || InZone(randTile.indexs, safeTile.indexs)))
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
    
    protected virtual void OnClickEvent(MineTile tile)
    {
        EventHandler handler = ClickEvent;
        handler?.Invoke(tile, EventArgs.Empty);
    }

    private void OnFlagTile(object sender, EventArgs e)
    {
        flagTiles.Add((MineTile) sender);
    }
    
    private void OnClickTile(object sender, EventArgs e)
    {
        MineTile clickedTile = (MineTile)sender;
        OnClickEvent(clickedTile);

        if (initialTile)
        {
            SetBoard(clickedTile, BOMB_COUNT);
        }

        if (clickedTile.isEmpty()) {
            clickedTile.SetIndex(AdjacentBombCount(clickedTile));
            RevealAdjacentEmptyTiles(clickedTile);
        }
        else if(clickedTile.isBomb()) {
            RevealAllBombs(clickedTile);
        }
    }

    private void RevealAllBombs(MineTile tile)
    {
        tile.SetIndex(-2);
        foreach (MineTile bombTile in bombTiles)
        {
            bombTile.Hidden = false;
        }
        foreach (MineTile flagTile in flagTiles)
        {
            if (!flagTile.isBomb())
                flagTile.ShouldDisplayWrongfulFlagged = true;
        }
    }
    
    private void RevealAdjacentEmptyTiles(MineTile clickedTile)
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
                        if (adjacentTile.Index == 0 && adjacentTile.Hidden)
                        {
                            adjacentTile.SetIndex(AdjacentBombCount(adjacentTile));
                            adjacentTile.Hidden = false;
                            OnClickTile(adjacentTile, EventArgs.Empty);
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