using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Minesweeper.Entities;
using Minesweeper.Extensions;
using Minesweeper.Particles;
using Minesweeper.System;

namespace Minesweeper.DataHolders;

public sealed class GridManager
{
    public readonly Timer celebrationParticleTimer = new Timer(0.5f, true);
    public GridTile[,] Grid { get; private set; }
    public GridTile this[int xindex, int yindex] => Grid[xindex, yindex];

    private bool initialTile = true;

    private readonly List<GridTile> bombTiles = new List<GridTile>();
    private readonly List<GridTile> flagTiles = new List<GridTile>();

    public bool RevealedBombs;
    public event EventHandler ClickEvent;
    public event EventHandler WinEvent;
    public event EventHandler LoseEvent;

    public int FlagCount => flagTiles.Count;

    private Main game;

    public GameConfig Config { get; private set; }
    private Vector2 Position { get; }

    public GridManager(Main game, Vector2 position)
    {
        this.game = game;
        Position = position;
        celebrationParticleTimer.FinishEvent += OnCelebrationParticleTimer;
    }

    private void OnCelebrationParticleTimer(object sender, EventArgs e)
    {
        if (game.GameState == GameState.Win)
        {
            GridTile[] aboveZ = Grid.Cast<GridTile>().Where(tile => tile.Index > 0).ToArray();
            if (aboveZ.Length > 0)
            {
                GridTile tile = aboveZ[Main.Random.Next(0, aboveZ.Length)];
                ParticleManager.SpawnTileParticle(tile, true);
                tile.Index = GridTile.EMPTY_INDEX;
            }
        }
    }

    public void Initialize(GameConfig config)
    {
        Config = config;
        Grid = new GridTile[Config.width, Config.height];
        // X
        for (int i = 0; i < Grid.GetLength(0); i++)
        {
            // Y
            for (int j = 0; j < Grid.GetLength(1); j++)
            {
                Grid[i, j] = new GridTile(game,
                    new Vector2(Position.X + i * GridTile.TILE_WIDTH, Position.Y + j * GridTile.TILE_HEIGHT),
                    GridTile.TILE_WIDTH, GridTile.TILE_HEIGHT, i, j);
                
                Grid[i, j].ClickEvent += OnClickTile;
                Grid[i, j].RevealEvent += OnRevealTile;
                Grid[i, j].FlagEvent += OnFlagTile;
            }
        }
    }

    public void ResetBoard()
    {
        initialTile = true;
        RevealedBombs = false;
        bombTiles.Clear();
        flagTiles.Clear();
        foreach (GridTile tile in Grid)
        {
            tile.SetIndex(0);
            tile.IsBadFlagged = false;
            tile.Flagged = false;
            tile.ClickedBomb = false;
            tile.Hidden = true;
        }
    }

    public void SetBombs(GridTile safeTile, int bombCount)
    {
        int tileCount = Grid.GetLength(0) * Grid.GetLength(1);
        int bombsPlaced = 0;
        while (bombsPlaced < Math.Clamp(bombCount, 0, tileCount - 1))
        {
            int randX = Main.Random.Next(Config.width);
            int randY = Main.Random.Next(Config.height);
            GridTile randTile = Grid[randX, randY];

            if (!randTile.IsBomb() &&
                !randTile.gridPosition.IsWithinAdjacentZone(safeTile.gridPosition, tileCount > 9 ? 1 : 0))
            {
                randTile.SetIndex(GridTile.BOMB_INDEX);
                bombsPlaced++;
                bombTiles.Add(randTile);
            }
        }

        initialTile = false;
    }

    private void OnClickEvent(GridTile tile, MouseButton button)
    {
        EventHandler handler = ClickEvent;
        handler?.Invoke(tile, new OnClickEventArgs(button));
    }

    private void OnWinEvent()
    {
        EventHandler handler = WinEvent;
        handler?.Invoke(this, EventArgs.Empty);
    }

    private void OnLoseEvent()
    {
        EventHandler handler = LoseEvent;
        handler?.Invoke(this, EventArgs.Empty);
    }

    private void OnFlagTile(object sender, EventArgs e)
    {
        GridTile tile = (GridTile)sender;
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
        GridTile clickedTile = (GridTile)sender;
        clickedTile.Hidden = false;
    }

    public void Win()
    {
        GridTile[] nonBombTiles = Grid.Cast<GridTile>().Where(tile => !tile.IsBomb()).ToArray();
        OnClickTile(nonBombTiles[Main.Random.Next(nonBombTiles.Length)], new OnClickEventArgs(MouseButton.Left));
        foreach (GridTile tile in Grid)
        {
            if (tile.IsEmpty())
            {
                OnClickTile(tile, new OnClickEventArgs(MouseButton.Left));
            }
        }
    }

    private void OnClickTile(object sender, OnClickEventArgs e)
    {
        var clickedTile = (GridTile)sender;
        var button = e.Action;
        var userClick = e.UserClick;

        switch (button)
        {
            case MouseButton.Left:
                HandleLeftClick(clickedTile, userClick);
                break;
            case MouseButton.Right:
                HandleRightClick(clickedTile);
                break;
        }
    }

    public void HandleLeftClick(GridTile tile, bool userClick)
    {
        OnClickEvent(tile, MouseButton.Left);
        if (RevealedBombs || tile.Flagged) return;

        if (initialTile)
        {
            SetBombs(tile, Config.bombCount);
            if (Config.showBombsAtStart)
                RevealAllBombs(tile);
        }

        if (tile.Hidden && userClick)
            ParticleManager.SpawnTileParticle(tile, false);

        if (tile.IsBomb())
        {
            HandleLose(tile);
            return;
        }

        if (tile.IsEmpty())
        {
            RevealEmptyTile(tile);
        }
        else if (userClick && !tile.Hidden && tile.Index > 0)
        {
            RevealAdjacentEmptyTilesWithFlags(tile);
        }
        
        if (Won())
        {
            HandleWin(tile);
        }
    }

    public void HandleRightClick(GridTile tile)
    {
        if (!tile.IsHidden() || RevealedBombs) return;

        Android.Service.Vibrate(100);

        if (tile.Flagged)
            ParticleManager.SpawnTileParticle(tile, true);

        tile.Flag();
    }

    private void RevealEmptyTile(GridTile tile)
    {
        tile.Reveal();
        tile.SetIndex(AdjacentBombCount(tile));
        RevealAdjacentEmptyTiles(tile, MouseButton.Left);
    }

    private void HandleLose(GridTile bombTile)
    {
        OnLoseEvent();
        game.GameState = GameState.Lose;
        RevealAllBombs(bombTile);
        RevealedBombs = true;
    }

    private void HandleWin(GridTile lastClickedTile)
    {
        OnWinEvent();
        game.GameState = GameState.Win;
        RevealAllBombs(lastClickedTile, true);
    }

    private bool Won()
    {
        bool allFound = true;
        foreach (GridTile tile in Grid)
        {
            if (!tile.IsBomb() && tile.Hidden)
                allFound = false;
        }

        return allFound;
    }

    private void RevealAllBombs(GridTile clickedTile, bool flagged = false)
    {
        if (!flagged)
        {
            if (clickedTile.IsBomb())
                clickedTile.ClickedBomb = true;

            foreach (GridTile bombTile in bombTiles)
            {
                if (!bombTile.Flagged)
                    bombTile.Hidden = false;
            }

            foreach (GridTile flagTile in flagTiles)
            {
                if (!flagTile.IsBomb())
                    flagTile.IsBadFlagged = true;
            }
        }
        else
        {
            foreach (GridTile bombTile in bombTiles)
            {
                if (!bombTile.Flagged)
                    flagTiles.Add(bombTile);

                bombTile.Flagged = true;
                bombTile.Hidden = true;
            }
        }
    }

    private void RevealAdjacentEmptyTilesWithFlags(GridTile clickedTile)
    {
        if (AdjacentFlagCount(clickedTile) != clickedTile.Index)
            return;

        int[] positions = [-1, 0, 1];
        foreach (int xPosition in positions)
        {
            foreach (int yPosition in positions)
            {
                int adjacentX = clickedTile.xIndex + xPosition;
                int adjacentY = clickedTile.yIndex + yPosition;

                if (adjacentX >= 0 && adjacentY >= 0 && adjacentX < Config.width && adjacentY < Config.height)
                {
                    GridTile adjacentTile = Grid[adjacentX, adjacentY];
                    if (adjacentTile.IsHidden() && !adjacentTile.Flagged)
                    {
                        if (game.Environment != GameEnvironments.Android)
                            ParticleManager.SpawnTileParticle(adjacentTile, false);
                        
                        if (!adjacentTile.IsBomb())
                            adjacentTile.SetIndex(AdjacentBombCount(adjacentTile));
                        adjacentTile.Hidden = false;
                        OnClickTile(adjacentTile, new OnClickEventArgs(MouseButton.Left, false));
                    }
                }
            }
        }
    }

    private void RevealAdjacentEmptyTiles(GridTile clickedTile, MouseButton button)
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
                    GridTile adjacentTile = Grid[adjacentX, adjacentY];
                    if (clickedTile.IsEmpty())
                    {
                        if (adjacentTile.IsEmpty() && adjacentTile.IsHidden() && !adjacentTile.Flagged)
                        {
                            if (game.Environment != GameEnvironments.Android)
                                ParticleManager.SpawnTileParticle(adjacentTile, false);
                            
                            adjacentTile.SetIndex(AdjacentBombCount(adjacentTile));
                            adjacentTile.Hidden = false;
                            OnClickTile(adjacentTile, new OnClickEventArgs(button, false));
                        }
                    }
                }
            }
        }
    }

    private int AdjacentFlagCount(GridTile tile)
    {
        int[] positions = { -1, 0, 1 };
        int flagCount = 0;

        foreach (int xPosition in positions)
        {
            foreach (int yPosition in positions)
            {
                int adjacentX = tile.xIndex + xPosition;
                int adjacentY = tile.yIndex + yPosition;

                if (adjacentX >= 0 && adjacentY >= 0 && adjacentX < Config.width && adjacentY < Config.height ||
                    (xPosition == 0 && yPosition == 0))
                {
                    GridTile adjacentTile = Grid[adjacentX, adjacentY];

                    if (adjacentTile.Flagged)
                    {
                        flagCount++;
                    }
                }
            }
        }

        return flagCount;
    }

    private int AdjacentBombCount(GridTile tile)
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
                    GridTile adjacentTile = Grid[adjacentX, adjacentY];

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