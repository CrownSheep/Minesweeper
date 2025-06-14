using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;

namespace Server;

public class TcpServer
{
    private TcpListener listener;
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
        listener.Start();
        Console.WriteLine($"Server started on port {Port}");

        while (running)
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient();
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

                // Click click = JsonConvert.DeserializeObject<Click>(line);
                // Console.WriteLine($"{click.Button} Click at ({click.X}, {click.Y})");
                //
                // string clickJson = JsonConvert.SerializeObject(click);
                //
                // lock (allClients)
                // {
                //     foreach (var c in allClients.ToList().Where(c => c != client))
                //     {
                //         try
                //         {
                //             var s = new StreamWriter(c.GetStream()) { AutoFlush = true };
                //             s.WriteLine(clickJson);
                //         }
                //         catch
                //         {
                //             // If failed, remove later
                //             allClients.Remove(c);
                //         }
                //     }
                // }
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
    }
}