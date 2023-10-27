// See https://aka.ms/new-console-template for more information

using Grpc.Core;

const int Port = 50051;

Server server = new Server
{
    Services = { WeatherForecast.WeatherForecasts.BindService(new DownloadService()) },
    Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
};

server.Start();

Console.WriteLine("Server listening on port " + Port);
Console.WriteLine("Press any key to stop the server...");
Console.ReadKey();

server.ShutdownAsync().Wait();

public class DownloadService : WeatherForecast.WeatherForecasts.WeatherForecastsBase
{
    public override async Task GetUpdateStream(WeatherForecast.DownloadRequest request, IServerStreamWriter<WeatherForecast.Update> responseStream, ServerCallContext context)
    {
        for (int i = 1; i <= 1000; i++)
        {
            await responseStream.WriteAsync(new WeatherForecast.Update() { ProgressInt = i });
            await Task.Delay(TimeSpan.FromSeconds(1)); // Simulate some processing time
        }
    }
}