using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using Minesweeper.DataHolders;
using Minesweeper.Entities;
using Newtonsoft.Json;

namespace Minesweeper;

public class TCPClient
{
    private static TcpClient client;
    private static NetworkStream stream;
    private static StreamReader reader;
    private static StreamWriter writer;

    private GameManager gameManager;

    public TCPClient(GameManager gameManager)
    {
        // string ngrokAddress = "4.tcp.eu.ngrok.io:14925";
        // string[] parts = ngrokAddress.Split(':');
        // string host = parts[0];
        // int port = int.Parse(parts[1]);
        //
        // client = new TcpClient(host, port);
        // stream = client.GetStream();
        // reader = new StreamReader(stream);
        // writer = new StreamWriter(stream) { AutoFlush = true };
        //
        // this.gameManager = gameManager;
        //
        // // Start listening on a background thread
        // Task.Run(ListenLoop);
    }

    public void SendUpdate(TileUpdate update)
    {
        SendUpdate(update.X, update.Y, update.State, update.User);
    }

    public void SendUpdate(int x, int y, TileUpdate.TileState state, bool user)
    {
        // TileUpdate update = new TileUpdate { X = x, Y = y, State = state, User = user};
        // string json = JsonConvert.SerializeObject(update);
        // writer.WriteLineAsync(json);
    }

    private async Task ListenLoop()
    {
        // try
        // {
        //     while (true)
        //     {
        //         string readUpdate = await reader.ReadLineAsync();
        //         if (string.IsNullOrEmpty(readUpdate)) break;
        //
        //         TileUpdate update = JsonConvert.DeserializeObject<TileUpdate>(readUpdate);
        //
        //         lock (gameManager.PendingUpdates)
        //         {
        //             gameManager.PendingUpdates.Enqueue(update);
        //             Console.WriteLine($"Enqueued update: {update.State} at ({update.X}, {update.Y})");
        //         }
        //     }
        // }
        // catch (Exception ex)
        // {
        //     Console.WriteLine("Connection lost: " + ex.Message);
        // }
    }
}