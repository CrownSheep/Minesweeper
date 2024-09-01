using System;
using Microsoft.Xna.Framework;

namespace Minesweeper.System;

public class Timer
{
    private float duration;
    private float remainingTime;

    private bool paused;
    private bool loop;
    
    public event EventHandler FinishEvent;

    public Timer(float duration, bool loop)
    {
        this.duration = duration;
        remainingTime = this.duration;
        this.loop = loop;
    }

    public void setDuration(float duration)
    {
        this.duration = duration;
    }
    
    public void setPaused(bool paused)
    {
        this.paused = paused;
    }
    
    public bool Finished()
    {
        return remainingTime is <= 0 and > -1;
    }
    
    public void End()
    {
        remainingTime = 0;
    }
    
    public void Reset()
    {
        remainingTime = duration;
    }

    public void Tick(GameTime gameTime)
    {
        float timer = (float) gameTime.ElapsedGameTime.TotalSeconds;

        if(!paused && remainingTime != -1)
            remainingTime -= timer;

        if(remainingTime is <= 0 and > -1)
        {
            OnFinishEvent();
            remainingTime = loop ? duration : -1;
        }
    }
    
    protected virtual void OnFinishEvent()
    {
        EventHandler handler = FinishEvent;
        handler?.Invoke(this, EventArgs.Empty);
    }
}