// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Hackathon2024.DTO;
using SocketIOClient;
using SocketIOClient.Transport;

using var socketIo = new SocketIOClient.SocketIO("https://games.uhno.de", new SocketIOOptions()
{
    Transport = TransportProtocol.WebSocket
});

socketIo.OnConnected += (sender, e) => Console.WriteLine("Connected");

socketIo.OnDisconnected += (sender, e) => Console.WriteLine("Disconnected");

await socketIo.ConnectAsync();

Console.WriteLine("Trying to Authenticate");

await socketIo.EmitAsync("authenticate", (success) => Console.WriteLine($"Authentication successful: {success}"), args[0]);

    socketIo.On("data", async (response) =>
    {
        using var game = response.GetValue<JsonDocument>();
        string? type = game.RootElement.GetProperty("type")
            .GetString();
        switch (type)
        {
            case "INIT":
                Console.WriteLine("Init");
                response.GetValue<Init>();
                break;
            case "RESULT":
                Console.WriteLine("Result");
                response.GetValue<Result>();
                break;
            case "ROUND":
                Console.WriteLine("Round");
                response.GetValue<Round>();
                break;
        }
    });
    using PeriodicTimer periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(5));
    while (await periodicTimer.WaitForNextTickAsync())
    {
        Console.WriteLine("Still Alive!");
    }