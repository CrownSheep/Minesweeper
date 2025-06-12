using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Minesweeper;

public class TcpServer
{
    private TcpListener listener;
    private Thread listenThread;
    private bool running = false;
    private readonly List<TcpClient> allClients = new();

    public int Port { get; private set; }

    public TcpServer(int port)
    {
        Port = port;
        listener = new TcpListener(IPAddress.Any, port);
    }

    public void Start()
    {
        running = true;
        listenThread = new Thread(() =>
        {
            listener.Start();
            Console.WriteLine($"Server started on port {Port}");

            while (running)
            {
                try
                {
                    var client = listener.AcceptTcpClient();
                    lock (allClients) allClients.Add(client);
                    Console.WriteLine("Client connected");

                    _ = HandleClientAsync(client); // fire and forget
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Accept error: {ex.Message}");
                }
            }

            listener.Stop();
        });

        listenThread.IsBackground = true;
        listenThread.Start();
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        await using var stream = client.GetStream();
        using var reader = new StreamReader(stream, Encoding.UTF8);
        await using var writer = new StreamWriter(stream, Encoding.UTF8);
        writer.AutoFlush = true;

        try
        {
            while (true)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null) break;

                // Deserialize the move
                TileUpdate update = JsonConvert.DeserializeObject<TileUpdate>(line);
                Console.WriteLine($"Received update: {update.State} at ({update.X}, {update.Y})");

                // Re-serialize to broadcast
                string updateJson = JsonConvert.SerializeObject(update);

                lock (allClients)
                {
                    
                    foreach (var c in allClients.ToList()) // avoid modification during loop
                    {
                        try
                        {
                            var s = new StreamWriter(c.GetStream()) { AutoFlush = true };
                            s.WriteLine(updateJson);
                        }
                        catch
                        {
                            // If failed, remove later
                            allClients.Remove(c);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Client error: {ex.Message}");
        }
        finally
        {
            client.Close();
            lock (allClients) allClients.Remove(client);
        }
    }
    
    public void Stop()
    {
        running = false;
        listener.Stop();
        lock (allClients)
        {
            foreach (var client in allClients)
                client.Close();
            allClients.Clear();
        }
        listenThread?.Join();
    }
}
