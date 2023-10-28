using Grpc.Core;
using LibSeedy;

const int Port = 50051;

Server server = new Server
{
    Services = { Seedly.Seedly.BindService(new DownloadService()) },
    Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
};

server.Start();

Console.WriteLine("Server listening on port " + Port);
Console.WriteLine("Press any key to stop the server...");
Console.ReadKey();

server.ShutdownAsync().Wait();