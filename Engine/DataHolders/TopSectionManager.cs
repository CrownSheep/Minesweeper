using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Minesweeper.System;

namespace Minesweeper.DataHolders;

public class TopSectionManager
{
    private const int MIN_INACTIVE_TIME = 10;
    public readonly Timer timer = new Timer(1, true);
    public int ElapsedSeconds { get; private set; }
    public int InactiveSeconds { get; private set; }
    public int LeftFlags => game.Config.bombCount - gridManager.FlagCount;

    private GridManager gridManager;

    private Main game;

    public TopSectionManager(Main game, GridManager gridManager)
    {
        this.game = game;
        timer.FinishEvent += OnSecondIncrement;
        timer.SetPaused(true);
        this.gridManager = gridManager;
        this.gridManager.ClickEvent += OnClickEvent;
    }

    public void Update()
    {
        if (game.IsActive)
        {
            timer.SetDuration(InactiveSeconds > MIN_INACTIVE_TIME ? 0.02f : 1f);
        }
    }
    
    private void OnSecondIncrement(object sender, EventArgs e)
    {
        if (!game.IsActive)
        {
            InactiveSeconds++;
            if (InactiveSeconds <= MIN_INACTIVE_TIME)
            {
                ElapsedSeconds++;
            }
        }
        else
        {
            if (InactiveSeconds > MIN_INACTIVE_TIME)
            {
                InactiveSeconds--;
            }
            else
            {
                InactiveSeconds = 0;
            }
            
            if(ElapsedSeconds < 999)
                ElapsedSeconds++;
        }
    }

    public void ResetTime()
    {
        ElapsedSeconds = 0;
    }
    
    private void OnClickEvent(object sender, EventArgs e)
    {
        GridTile tile = (GridTile) sender;
        OnClickEventArgs clickEventArgs = (OnClickEventArgs) e;
        if (clickEventArgs.Button == MouseButton.Left)
        {
            if(gridManager.RevealedBombs)
                return;
            
            if (!tile.Flagged)
                timer.SetPaused(false);
        }
    }
    
}