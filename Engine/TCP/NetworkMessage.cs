namespace Minesweeper;

public class NetworkMessage
{
    public MessageType Type { get; set; }
    public string PayloadJson { get; set; } = string.Empty;
}
