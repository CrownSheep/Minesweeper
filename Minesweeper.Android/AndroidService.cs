#nullable enable
using Android.App;
using Android.Content;
using Android.OS;
using Android.Util;
using Microsoft.Xna.Framework;

namespace Minesweeper.Android;

public class AndroidService : Main.IAndroidService
{
    public void Vibrate(long milliseconds, int amplitude = 1)
    {
        Vibrate(VibrationEffect.CreateOneShot(milliseconds, amplitude));
    }
    
    public void Vibrate(VibrationEffect? effect)
    {
        var activity = (Activity)Game.Activity;
        var vibrator = (Vibrator)activity.GetSystemService(Context.VibratorService)!;
        
        if (vibrator.HasVibrator)
            vibrator.Vibrate(effect);
    }
    
    public void ConsoleLog(string? prefix, string message)
    {
        Log.Debug(prefix, message);
    }
}