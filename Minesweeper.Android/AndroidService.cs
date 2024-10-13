using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Microsoft.Xna.Framework;

namespace Minesweeper.Android;

public class AndroidService : Main.IAndroidService
{
    public void Vibrate(long milliseconds, int amplitude = 0)
    {
        var activity = (Activity)Game.Activity;
        var vibrator = (Vibrator)activity.GetSystemService(Context.VibratorService);
        
        if (vibrator != null && vibrator.HasVibrator)
        {
            if(vibrator.HasAmplitudeControl)
                vibrator.Vibrate(VibrationEffect.CreateOneShot(milliseconds, amplitude));
                
            // vibrator.Vibrate(milliseconds);
        }
    }
    
    public void ConsoleLog(string? prefix, string message)
    {
        Log.Debug(prefix, message);
    }
}