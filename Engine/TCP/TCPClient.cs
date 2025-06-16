using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Minesweeper.System.Input.Global;
using Newtonsoft.Json;

namespace Minesweeper;

public class TCPClient
{
    private TcpClient client;
    private NetworkStream stream;
    private StreamReader reader;
    private StreamWriter writer;

    public TCPClient()
    {
        string ngrokAddress = "4.tcp.eu.ngrok.io:16631";
        NgrokAddress address = new NgrokAddress(ngrokAddress);

        try
        {
            client = new TcpClient(address.Host, address.Port);
            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };
        
            Task.Run(ListenLoop);
        }
        catch (Exception e)
        {
            Console.WriteLine("Server not running!");
        }
    }

    public void SendClick(float x, float y, PointerAction action)
    {
        // Click click = new Click(x, y, action);
        // string json = JsonConvert.SerializeObject(click);
        // writer.WriteLineAsync(json);
    }

    private async Task ListenLoop()
    {
        try
        {
            while (true)
            {
                string readClick = await reader.ReadLineAsync();
                if (string.IsNullOrEmpty(readClick)) break;

                // Click click = JsonConvert.DeserializeObject<Click>(readClick);
                // clientClick = click;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Connection lost: " + ex.Message);
        }
    }
}