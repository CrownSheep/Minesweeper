using System;
using Microsoft.Xna.Framework;

namespace Minesweeper;

public static class ScreenSpaceUtils
{
    public static Vector2 ToCommonPosition(Vector2 position, bool round = false)
    {
        Vector2 scaledPos = position / Main.ZoomFactor;
        
        if(round)
            scaledPos = new Vector2((float) Math.Round(scaledPos.X), (float) Math.Round(scaledPos.Y));
        
        return scaledPos;
    }
    
    public static Vector2 ToZoomedPosition(Vector2 position)
    {
        Vector2 scaledPos = position * Main.ZoomFactor;
        return scaledPos;
    } 
}