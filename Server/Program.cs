// See https://aka.ms/new-console-template for more information

using Server;

const int PORT = 5000;

TcpServer tcpServer = new TcpServer(PORT);
tcpServer.Start();