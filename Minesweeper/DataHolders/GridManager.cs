using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Minesweeper.Entities;

namespace Minesweeper.DataHolders;

public class GridManager
{
    public TileManager[,] Grid { get; private set; }

    private Texture2D spriteSheet;

    private bool initialTile = true;

    private Random random;

    private List<TileManager> bombTiles = new List<TileManager>();
    private List<TileManager> flagTiles = new List<TileManager>();

    public bool revealedBombs;
    public event EventHandler ClickEvent;

    public int FlagCount => flagTiles.Count;
    public TileManager this[int xindex, int yindex] => Grid[xindex, yindex];

    private Game1 game;

    public GameConfig Config { get; private set; }

    private Vector2 Position { get; }

    public GridManager(Game1 game, Texture2D spriteSheet, Vector2 position)
    {
        this.game = game;
        this.spriteSheet = spriteSheet;
        random = new Random();
        Position = position;
    }

    public void Initialize(GameConfig config)
    {
        Config = config;
        Grid = new TileManager[Config.width, Config.height];
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Grid[i, j] = new TileManager(game, spriteSheet,
                    new Vector2(Position.X + i * TileManager.TILE_WIDTH, Position.Y + j * TileManager.TILE_HEIGHT),
                    TileManager.TILE_WIDTH, TileManager.TILE_HEIGHT, i, j);
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
        foreach (TileManager tile in Grid)
        {
            tile.SetIndex(0);
            tile.IsBadFlagged = false;
            tile.Flagged = false;
            tile.ClickedBomb = false;
            tile.Hidden = true;
        }
    }

    public void SetBombs(TileManager safeTile, int bombCount)
    {
        int bombsPlaced = 0;
        while (bombsPlaced < bombCount)
        {
            int randX = random.Next(Config.width);
            int randY = random.Next(Config.height);
            TileManager randTile = Grid[randX, randY];
            if (!randTile.IsBomb() && !InZone(randTile.indexs, safeTile.indexs))
            {
                randTile.SetIndex(TileManager.BOMB_INDEX);
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

    protected virtual void OnClickEvent(TileManager tile, ButtonState buttonState)
    {
        EventHandler handler = ClickEvent;
        handler?.Invoke(tile, new OnClickEventArgs(buttonState));
    }

    private void OnFlagTile(object sender, EventArgs e)
    {
        TileManager tile = (TileManager)sender;
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
        TileManager clickedTile = (TileManager)sender;
        clickedTile.Hidden = false;
    }

    private void OnClickTile(object sender, EventArgs e)
    {
        TileManager clickedTile = (TileManager)sender;
        ButtonState button = ((OnClickEventArgs)e).Button;
        
        if (button == Mouse.GetState().LeftButton)
        {
            OnClickEvent(clickedTile, button);
            if (!revealedBombs && !clickedTile.Flagged)
            {
                if (initialTile)
                {
                    SetBombs(clickedTile, Config.bombCount);
                    if(Config.showBombsAtStart)
                        RevealAllBombs(clickedTile);
                }

                if (clickedTile.IsEmpty())
                {
                    clickedTile.Reveal();
                    clickedTile.SetIndex(AdjacentBombCount(clickedTile));
                    RevealAdjacentEmptyTiles(clickedTile, button);
                    if (Won())
                    {
                        foreach (TileManager bombTile in bombTiles)
                        {
                            if(!bombTile.Flagged)
                                flagTiles.Add(bombTile);
                            
                            bombTile.Flagged = true;
                            bombTile.Hidden = true;
                        }
                    }
                }
                else if (clickedTile.IsBomb())
                {
                    RevealAllBombs(clickedTile);
                    revealedBombs = true;
                }
            }
        }

        if (button == Mouse.GetState().RightButton)
        {
            if (clickedTile.IsHidden() && !revealedBombs)
                clickedTile.Flag();
        }
    }

    private bool Won()
    {
        bool allFound = true;
        foreach (TileManager tile in Grid)
        {
            if (!tile.IsBomb() && tile.Hidden)
                allFound = false;
        }

        return allFound;
    }

    private void RevealAllBombs(TileManager clickedTile)
    {
        if(clickedTile.IsBomb())
            clickedTile.ClickedBomb = true;
        
        foreach (TileManager bombTile in bombTiles)
        {
            if (!bombTile.Flagged)
                bombTile.Hidden = false;
        }

        foreach (TileManager flagTile in flagTiles)
        {
            if (!flagTile.IsBomb())
                flagTile.IsBadFlagged = true;
        }
    }

    private void RevealAdjacentEmptyTiles(TileManager clickedTile, ButtonState buttonState)
    {
        int[] positions = { -1, 0, 1 };
        foreach (int xPosition in positions)
        {
            foreach (int yPosition in positions)
            {
                int adjacentX = clickedTile.xIndex + xPosition;
                int adjacentY = clickedTile.yIndex + yPosition;

                if (adjacentX >= 0 && adjacentY >= 0 && adjacentX < Config.width && adjacentY < Config.height)
                {
                    TileManager adjacentTile = Grid[adjacentX, adjacentY];
                    if (clickedTile.IsEmpty())
                    {
                        if (adjacentTile.IsEmpty() && adjacentTile.IsHidden())
                        {
                            adjacentTile.SetIndex(AdjacentBombCount(adjacentTile));
                            adjacentTile.Hidden = false;
                            OnClickTile(adjacentTile, new OnClickEventArgs(buttonState));
                        }
                    }
                }
            }
        }
    }

    private int AdjacentBombCount(TileManager tile)
    {
        int[] positions = { -1, 0, 1 };
        int bombCount = 0;

        foreach (int xPosition in positions)
        {
            foreach (int yPosition in positions)
            {
                int adjacentX = tile.xIndex + xPosition;
                int adjacentY = tile.yIndex + yPosition;

                if (adjacentX >= 0 && adjacentY >= 0 && adjacentX < Config.width && adjacentY < Config.height ||
                    (xPosition == 0 && yPosition == 0))
                {
                    TileManager adjacentTile = Grid[adjacentX, adjacentY];

                    if (adjacentTile.IsBomb())
                    {
                        bombCount++;
                    }
                }
            }
        }

        return bombCount;
    }
}