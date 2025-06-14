namespace Minesweeper;

public class NgrokAddress
{
    public string Host { get; }
    public int Port { get; }
    
    public NgrokAddress(string address)
    {
        string[] parts = address.Split(':');
        Host = parts[0];
        Port = int.Parse(parts[1]);
    }
}