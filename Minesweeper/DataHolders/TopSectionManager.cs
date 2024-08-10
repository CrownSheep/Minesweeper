using System;
using Microsoft.Xna.Framework.Input;
using Minesweeper.System;

namespace Minesweeper.DataHolders;

public class TopSectionManager
{
    public readonly Timer timer = new Timer(1, true);
    public int ElapsedSeconds { get; private set; }
    public int LeftFlags => GridManager.BOMB_COUNT - gridManager.FlagCount;

    private GridManager gridManager;

    public TopSectionManager(GridManager gridManager)
    {
        timer.FinishEvent += OnSecondIncrement;
        timer.setPaused(true);
        this.gridManager = gridManager;
        this.gridManager.ClickEvent += OnClickEvent;
    }
    
    private void OnSecondIncrement(object sender, EventArgs e)
    {
        ElapsedSeconds++;
    }

    public void ResetTime()
    {
        ElapsedSeconds = 0;
    }
    
    private void OnClickEvent(object sender, EventArgs e)
    {
        TileManager tile = (TileManager) sender;
        OnClickEventArgs clickEventArgs = (OnClickEventArgs) e;
        if (clickEventArgs.Button == Mouse.GetState().LeftButton)
        {
            if(gridManager.revealedBombs)
                return;
            
            if (!tile.Flagged)
                timer.setPaused(false);

            if (tile.IsBomb())
                timer.setPaused(true);
        }
    }
    
}