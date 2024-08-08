using System;
using Microsoft.Xna.Framework;

namespace Minesweeper.System;

public class Timer
{
    private float duration;
    private float remainingTime;

    private bool paused;
    
    public event EventHandler FinishEvent;

    public Timer(float duration)
    {
        this.duration = duration;
        remainingTime = this.duration;
    }

    public void setDuration(float duration)
    {
        this.duration = duration;
    }
    
    public void setPaused(bool paused)
    {
        this.paused = paused;
    }
    
    public void End()
    {
        remainingTime = 0;
    }

    public void Tick(GameTime gameTime)
    {
        float timer = (float) gameTime.ElapsedGameTime.TotalSeconds;

        if(!paused)
            remainingTime -= timer;

        if(remainingTime <= 0)
        {
            OnFinishEvent();
            remainingTime = duration;
        }
    }
    
    protected virtual void OnFinishEvent()
    {
        EventHandler handler = FinishEvent;
        handler?.Invoke(this, EventArgs.Empty);
    }
}