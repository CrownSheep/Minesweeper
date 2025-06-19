using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Minesweeper.DataHolders;
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

    public TcpClient Client => client;
    public NgrokAddress Address { get; private set; }

    // public Action<PointerSnapshot[]>? OnInputReceived;

    public TCPClient(string address)
    {
        try
        {
            Address = new NgrokAddress(address);
            client = new TcpClient(Address.Host, Address.Port);

            stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };

            Task.Run(ListenLoop);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to connect to server: {ex}");
        }
    }

    // public async void SendInput(PointerSnapshot[] snapshots)
    // {
    //     try
    //     {
    //         string json = JsonConvert.SerializeObject(snapshots);
    //         await writer.WriteLineAsync(json);
    //     }
    //     catch (Exception e)
    //     {
    //         Console.WriteLine($"Failed to send input to server: {e}"); 
    //     }
    // }

    public async void SendMessage<T>(MessageType type, T payload)
    {
        try
        {
            var message = new NetworkMessage
            {
                Type = type,
                PayloadJson = JsonConvert.SerializeObject(payload)
            };

            string json = JsonConvert.SerializeObject(message);
            await writer.WriteLineAsync(json);
        }
        catch (Exception e)
        {
            // ignored
        }
    }


    private async Task ListenLoop()
    {
        try
        {
            while (true)
            {
                string? json = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(json)) break;

                NetworkMessage? message = JsonConvert.DeserializeObject<NetworkMessage>(json);

                switch (message?.Type)
                {
                    case MessageType.BoardSeed:
                    {
                        int seed = JsonConvert.DeserializeObject<int>(message.PayloadJson);
                        Egg.SetSeed(seed);
                        
                        break;
                    }
                    case MessageType.PointerInput:
                    {
                        PointerSnapshot[]? snapshots =
                            JsonConvert.DeserializeObject<PointerSnapshot[]>(message.PayloadJson);

                        if (snapshots == null) continue;

                        RemoteInput.UpdateSnapshots(snapshots);
                        
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Connection lost: " + ex.Message);
        }
    }
}