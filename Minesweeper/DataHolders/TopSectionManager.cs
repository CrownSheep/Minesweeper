using System;
using Microsoft.Xna.Framework.Input;
using Minesweeper.GameElements;
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
    
    private void OnClickEvent(object sender, EventArgs e)
    {
        MineTile tile = (MineTile) sender;
        OnClickEventArgs clickEventArgs = (OnClickEventArgs) e;
        if (clickEventArgs.Button == Mouse.GetState().LeftButton)
        {
            if (!tile.Flagged)
                timer.setPaused(false);

            if (tile.isBomb())
                timer.setPaused(true);
        }
    }
    
}