// See https://aka.ms/new-console-template for more information

using Server;

const int PORT = 5000;

TcpServer server = new TcpServer(PORT);
server.Start();
Console.WriteLine("Press Enter to exit...");
Console.ReadLine();
server.Stop();