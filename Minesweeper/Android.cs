namespace Minesweeper;

public static class Android
{
    private class DefaultAndroidService : Main.IAndroidService
    {
        public void Vibrate(long milliseconds, int amplitude = 1)
        {
            
        }

        public void ConsoleLog(string prefix, string message)
        {
            
        }
    }
    public static Main.IAndroidService Service = new DefaultAndroidService();
}