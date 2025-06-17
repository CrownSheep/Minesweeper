using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Minesweeper.System.Input.Global;
using Minesweeper.System.Input.Pointer.Remote;
using Newtonsoft.Json;

namespace Minesweeper;

public class TCPClient
{
    private TcpClient client;
    private NetworkStream stream;
    private StreamReader reader;
    private StreamWriter writer;

    private PointerSnapshot[] lastSnapshots;

    public Action<PointerSnapshot[]>? OnInputReceived;

    public TCPClient(string address)
    {
        try
        {
            NgrokAddress parsed = new NgrokAddress(address);
            client = new TcpClient(parsed.Host, parsed.Port);

            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            Task.Run(ListenLoop);
        }
        catch
        {
            Console.WriteLine("Failed to connect to server.");
        }
    }

    public async void SendInput(PointerSnapshot[] snapshots)
    {
        try
        {
            string json = JsonConvert.SerializeObject(snapshots);
            Console.WriteLine(json);
            await writer.WriteLineAsync(json);
            lastSnapshots = snapshots;
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed to send input to server.");
        }
    }
    

    private async Task ListenLoop()
    {
        try
        {
            while (true)
            {
                string? line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) break;

                PointerSnapshot[]? snapshots = JsonConvert.DeserializeObject<PointerSnapshot[]>(line);
                
                if (snapshots == null) continue;
                
                RemoteInput.FromServer(snapshots);
                OnInputReceived?.Invoke(snapshots);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Connection lost: " + ex.Message);
        }
    }
}
