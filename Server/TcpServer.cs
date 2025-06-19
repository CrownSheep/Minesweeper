using System.Net;
using System.Net.Sockets;

namespace Server;

public class TcpServer(int port)
{
    private TcpListener listener = new(IPAddress.Any, port);
    private bool running = false;

    private readonly List<ClientWrapper> allClients = new();

    public int Port { get; private set; } = port;

    public void Start()
    {
        running = true;
        listener.Start();
        Console.WriteLine($"Server started on port {Port}");
        _ = AcceptClientsAsync();
    }

    private async Task AcceptClientsAsync()
    {
        while (running)
        {
            try
            {
                TcpClient client = await listener.AcceptTcpClientAsync();
                var wrapper = new ClientWrapper(client);
                lock (allClients) allClients.Add(wrapper);
                Console.WriteLine($"Client connected: {client.Client.RemoteEndPoint}");

                _ = HandleClientAsync(wrapper);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Accept error: {ex.Message}");
            }
        }
    }

    private async Task HandleClientAsync(ClientWrapper wrapper)
    {
        var client = wrapper.Client;
        var reader = wrapper.Reader;
        
        try
        {
            while (true)
            {
                string? line = await reader.ReadLineAsync();
                if (line == null) break;
                
                Console.WriteLine(line);
                
                // Send to other clients
                lock (allClients)
                {
                    foreach (var other in allClients.Where(c => c != wrapper).ToList())
                    {
                        try
                        {
                            other.Writer.WriteLine(line);
                        }
                        catch
                        {
                            other.Client.Close();
                            allClients.Remove(other);
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
            Console.WriteLine($"Client disconnected: {client.Client.RemoteEndPoint}");
            client.Close();
            lock (allClients) allClients.Remove(wrapper);
        }
    }

    public void Stop()
    {
        running = false;
        listener.Stop();
        lock (allClients)
        {
            foreach (var wrapper in allClients)
                wrapper.Client.Close();

            allClients.Clear();
        }
    }

    private class ClientWrapper
    {
        public TcpClient Client { get; }
        public StreamWriter Writer { get; }
        public StreamReader Reader { get; }

        public ClientWrapper(TcpClient client)
        {
            Client = client;
            var stream = client.GetStream();
            Reader = new StreamReader(stream);
            Writer = new StreamWriter(stream) { AutoFlush = true };
        }
    }
}
